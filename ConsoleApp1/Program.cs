using DIContainer;

var serviceBuilder = new ServiceCollectionBuilder();
serviceBuilder.AddTransient<IClass1, Class1>().AddTransient<Class2, Class2>();
var serviceCollection = serviceBuilder.Build();
var scope = serviceCollection.CreateScope();
var item1 = scope.RealizedService<IClass1>();
var item2 = scope.RealizedService<IClass1>();

Console.WriteLine(item1.Equals(item2));
Console.ReadKey();


interface IClass1
{
    
}

class Class1: IClass1
{
    public Class1()
    {
        
    }
}

class Class2
{
    public IClass1 Class1 { get; set; }
    
    public Class2(IClass1 class1)
    {
        
    }
}