using AddressBookSys.Models.Entities;
using AddressBookSys.Models.Repositories;
using AddressBookSys.Models.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORSポリシーを追加
builder.Services.AddCors(options =>
{
    // 検証用※本番利用不可！
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });

    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5289") // Blazor PWAのURLを指定
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services
    // .AddDbContext<AddressBookContext>(x => x.UseSqlite(connection))
    // .AddDbContext<AddressBookContext>(x => x.UseNpgsql(connectionString), ServiceLifetime.Singleton, ServiceLifetime.Singleton)
    // .AddSingleton<IAddressBookRepository, AddressBookRepository>()
    .AddSingleton<IAddressBookRepository, AddressBookRepositoryMoc>()
    .AddTransient<IAddressBookService, AddressBookService>();

var app = builder.Build();

// CORSをミドルウェアに適用
// app.UseCors("AllowSpecificOrigin");
app.UseCors("AllowAll"); // 検証用※本番利用不可！

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // DB作成
    // var context = services.GetRequiredService<AddressBookContext>();
    // var addressBookRepository = new AddressBookRepository(context);
    // await addressBookRepository.DatabaseEnsureCreated();

    // ダミーデータ追加
    var addressBookService = services.GetRequiredService<IAddressBookService>();
    foreach(var x in new[]{
        ("東京 太郎", "tokyo_taro@example.com"),
        ("大阪 花子", "osaka_hanako@example.com"),
        ("京都 次郎", "kyoto_jiro@example.com"),
        ("名古屋 美咲", "nagoya_misaki@example.com"),
        ("札幌 健", "sapporo_ken@example.com"),
        ("福岡 由美", "fukuoka_yumi@example.com"),
        ("横浜 大輔", "yokohama_daisuke@example.com"),
        ("神戸 優子", "kobe_yuko@example.com"),
        ("仙台 忠", "sendai_tadashi@example.com"),
        ("広島 美紀", "hiroshima_miki@example.com"),
        ("東京 花子", "tokyo_hanako@example.com"),
        ("大阪 太郎", "osaka_taro@example.com"),
        ("京都 美咲", "kyoto_misaki@example.com"),
        ("名古屋 健", "nagoya_ken@example.com"),
        ("札幌 由美", "sapporo_yumi@example.com"),
        ("福岡 次郎", "fukuoka_jiro@example.com"),
        ("横浜 優子", "yokohama_yuko@example.com"),
        ("神戸 大輔", "kobe_daisuke@example.com"),
        ("仙台 美紀", "sendai_miki@example.com"),
        ("広島 忠", "hiroshima_tadashi@example.com"),
        ("東京 次郎", "tokyo_jiro@example.com"),
        ("大阪 美咲", "osaka_misaki@example.com"),
        ("京都 健", "kyoto_ken@example.com"),
        ("名古屋 由美", "nagoya_yumi@example.com"),
        ("札幌 太郎", "sapporo_taro@example.com"),
        ("福岡 美紀", "fukuoka_miki@example.com"),
        ("横浜 忠", "yokohama_tadashi@example.com"),
        ("神戸 花子", "kobe_hanako@example.com"),
        ("仙台 大輔", "sendai_daisuke@example.com"),
        ("広島 優子", "hiroshima_yuko@example.com"),
    }) {
        await addressBookService.AddAddressBook(new AddressBook(0, x.Item1, x.Item2));
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
