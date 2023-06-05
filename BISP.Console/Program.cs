using BISP.Infra.EfCore;
using BISP.Infra.Entity.Data;
using BISP.Infra.Entity.Entities;
using BISP.Service.IRepository;
using Microsoft.EntityFrameworkCore;


string connectionString = "Server=localhost;port=5432;Database=BISP;User Id=postgres;Password=;";
IRepository<Recipe> _repository;

// 連線
var options = new DbContextOptionsBuilder<BispContext>()
    .UseNpgsql(connectionString)
    .Options;

_repository = new EfRepository<Recipe>(new BispContext(options));


// 新增項目
for (int i = 0; i < 3; i++)
{
    await _repository.InsertAsync(new Recipe { Guid = Guid.NewGuid(), ItemName = "Jed", ItemValue = 100 * i, CreateAt = DateTime.Now });
}


// 查詢所有項目
IEnumerable<Recipe> allPeople = await _repository.GetAllAsync();


// 根據 GUID 查詢指定項目（注意：這裡的 Guid 字串，要根據資料庫內部有的）
Recipe recipe = await _repository.GetByIdAsync(new Guid("d451fa9a-78ae-4f05-98f9-e463021f304d"));



// 更新項目
recipe.ItemName = "Update";
recipe.ItemValue = 555;
await _repository.Update(recipe);


// 刪除項目
_repository.Delete(new Guid("d451fa9a-78ae-4f05-98f9-e463021f304d"));


Console.ReadKey();
