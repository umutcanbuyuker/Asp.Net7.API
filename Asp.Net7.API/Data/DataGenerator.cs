using Asp.Net7.API.Core;
using Asp.Net7.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Asp.Net7.API.Data
{
    public class DataGenerator : IDataGenerator
    {
        private readonly ApiDbContext _context;
        public DataGenerator(ApiDbContext context)
        {
            _context = context;
        }


        /* Neden In-Memory Database ile çalışmalıyız? 
           - Genellikle bu özelliği yeni çıkan EF Core özelliklerini test edebilmek için kullanıyorum.
           - EF Core, fiziksel veritabanlarından ziyada in-kmemory'de Database oluşturup üzerinde birçok işlemi yapmamızı sağlayabilmektedir. İşte bu özellik ile gerçek uygulamaların dışında test gibi operasyonları hızlıca yürütebileceğimiz imkanlar elde edebilmekteyiz. */

        /* Avantajları nelerdir?
             - Test ve pre-prod uygulamarda gerçek/fiziksel veritabanları oluşturmak ve yapılandırmak yerine tüm veritabanını bellekte modelleyebilir ve gerekli işlemleri sanki gerçek bir veritabanında çalışıyor gibi orada gerçekleştirebiliriz.
             - Bellekte çalışmak geçici bir deneyim olacağı için veritabanı serverlarında test amaçlı üretilmiş olan veritabanlarının lüzumsuz yer işgal etmesini engellemiş olacaktır.
             - Bellekte veritabanını modellemek kodun hızlı bir şekilde test edilmesini sağlayacaktır.
             - In-Memory database üzerinde çalışırken migration oluşturmaya ve migrate etmeye gerek yoktur.
         */

        /* Dezavantajları nelerdir?
             - In-Memory'de yapılacak olan veritabanı işlevlerinde ilişkisel modellemeler yapılamamaktadır. Bu durumdan dolayı veri tutarlılığı sekteye uğrayabilir ve istatiksel açıdan yanlış sonuçlar elde edilebilir.
             - In-Memory'de oluşturulmuş olan database uygulama sona erdiği takdirde bellekten silinecektir.
        */

        public void Initialize()
        {
            // toDos içerisinde bir eleman var mı diye kontrol sağlıyoruz
            if (_context.toDos.Any())
            {
                return;
            }
            _context.toDos.AddRange(
                    new ToDo
                    {
                        //Id = 1,
                        Name = "SignalR",
                        Category = "iş",
                        PublishDate = new DateTime(2023, 02, 10)
                    },
                    new ToDo
                    {
                        //Id = 2,
                        Name = "Redis",
                        Category = "iş",
                        PublishDate = new DateTime(2023, 02, 20)
                    },
                    new ToDo
                    {
                        //Id = 3,
                        Name = "RabbitMQ",
                        Category = "iş",
                        PublishDate = new DateTime(2023, 02, 25)
                    }
                );
            _context.SaveChanges(); 
        }

    }
}

