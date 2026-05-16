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
        public void Intialize()
        {
            try
            {
                //Check if Tables Is Empty Or Not عشان ادخله الداتا => So Need object From DbContext
                var hasProduct = _storeDBContext.Products.Any();
                var hasBrand = _storeDBContext.productBrands.Any();
                var HasTypes = _storeDBContext.productTypes.Any();
                if (hasProduct && hasBrand && HasTypes)
                {
                    return;//كدة معناناها فى Data in Tables كدة مش هينفع اعمل Seeding For Data 
                }//طب لو مفيش عايز بقا ادخل الداتا دى جوه الTables


                //Product has Relation with Brand + Types ومش هينفع ادخل الProduct الاول لازم ادخله Types الاول + Brand عشان يعرف الForign Key تنزله عادى بالتالى مش هعرف ادخله داتا هو الاول لازم ادخل Data For Types+ Brand 
                if (!hasBrand)
                {
                    //اقرا الداتا + AddRange
                    SeedDataFromJson<ProductBrand, int>("brands.json",_storeDBContext.productBrands);
                }
                if (!HasTypes)
                {
                    //Read Data + AddRange
                    SeedDataFromJson<ProductType, int>("types.json", _storeDBContext.productTypes);

                }
                _storeDBContext.SaveChanges();  
                //محتاج Savehcnages in Database الاول 
                if (!hasProduct)
                {
                    SeedDataFromJson<Product, int>("products.json", _storeDBContext.Products);
                    _storeDBContext.SaveChanges();
                }
              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Accured During Seeding Data {ex}");
            }
        }
        private void SeedDataFromJson<T, Tkey>(string filename, DbSet<T> dbset) where T : BaseEntity<Tkey>
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
                var Data = JsonSerializer.Deserialize<List<T>>(DataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,//To Ignore Case Senstives 
                });//Read From Stream then Convert this To ListOfT
                if (Data is not null)
                {
                    dbset.AddRange(Data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While Reading Data From Json {ex}");
            }
        }
    }
}
