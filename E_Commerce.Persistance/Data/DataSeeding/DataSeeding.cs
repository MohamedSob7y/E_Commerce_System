using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes;
using E_Commerce.Persistance.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Data.DataSeeding
{
    public class DataSeeding : IDataSeeding
    {
        private readonly StoreDBContext _storeDBContext;

        public DataSeeding(StoreDBContext storeDBContext)
        {
            _storeDBContext = storeDBContext;
        }
        public async Task IntializeAsync()
        {
            try
            {
                //Check if Tables Is Empty Or Not عشان ادخله الداتا => So Need object From DbContext
                var hasProduct = await _storeDBContext.Products.AnyAsync();//this Database hits must be work as Ascync لان انا بحول اى Function to Async فى تلت حالات 
                //1: Database Operation Hits  2:External Api Call    3: File input or output Reader
                //any Function Work as Async => Return Type is Task 
                //All Linq Method has method With Async  
                var hasBrand = await _storeDBContext.productBrands.AnyAsync();
                var HasTypes = await _storeDBContext.productTypes.AnyAsync();
                if (hasProduct && hasBrand && HasTypes)
                {
                    return;//كدة معناناها فى Data in Tables كدة مش هينفع اعمل Seeding For Data 
                }//طب لو مفيش عايز بقا ادخل الداتا دى جوه الTables


                //Product has Relation with Brand + Types ومش هينفع ادخل الProduct الاول لازم ادخله Types الاول + Brand عشان يعرف الForign Key تنزله عادى بالتالى مش هعرف ادخله داتا هو الاول لازم ادخل Data For Types+ Brand 
                if (!hasBrand)
                {
                    //اقرا الداتا + AddRange
                    await SeedDataFromJson<ProductBrand, int>("brands.json", _storeDBContext.productBrands);
                }
                if (!HasTypes)
                {
                    //Read Data + AddRange
                    await SeedDataFromJson<ProductType, int>("types.json", _storeDBContext.productTypes);

                }
                await _storeDBContext.SaveChangesAsync();
                //محتاج Savehcnages in Database الاول 
                if (!hasProduct)
                {
                    await SeedDataFromJson<Product, int>("products.json", _storeDBContext.Products);
                    await _storeDBContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Accured During Seeding Data {ex}");
            }
        }
        private async Task SeedDataFromJson<T, Tkey>(string filename, DbSet<T> dbset) where T : BaseEntity<Tkey>
            //Read DataFromJson+AddRang DataLocal in Database
        {
            //this Full Path of FIles Brand=> F:\Projects\E_Commerce_System\E_Commerce.Persistance\Data\Json Files\brands.json
            //Default Path => Layer اللى بتعمل Run
            var filepath = @"..\E_Commerce.Persistance\Data\Json Files\" + filename;//.. دى معنانا خرجنى برة Web.api
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException("Json File not Found", filepath);
            }
            //Read Data after Checking Pathof File
            try
            {
                //var Data = File.ReadAllText(filepath);//Read Data As String ولو الفايلات كبيرة هتبقى مشكلة =>So Open Stream with File To REad Data as Bytes When Serializing عشان مش عايز اعمل Load Data in Ram
                var DataStream = File.OpenRead(filepath);//Read File From Stream 
                var Data = await JsonSerializer.DeserializeAsync<List<T>>(DataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,//To Ignore Case Senstives 
                });//Read From Stream then Convert this To ListOfT
                if (Data is not null)
                {
                    await dbset.AddRangeAsync(Data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While Reading Data From Json {ex}");
            }
        }
    }
}
