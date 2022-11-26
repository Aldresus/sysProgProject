using System;
using System.Resources;

public class V_Create
{
	private viewModel oViewModel;
    
	public V_Create(viewModel VM)
	{
        //TODO language
        this.oViewModel = VM;
        //TODO check if there is less than 5 save jobs
        Console.Clear();
        Console.WriteLine("job ?");
        string job = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("source ?");
        string source = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("dest ?");
        string dest = Console.ReadLine();
        Console.Clear();
        Console.WriteLine(@"type ?
            1 ful
            2 differential");
        string type = Console.ReadLine();
        Console.Clear();
        Console.WriteLine(@$"job: {job}
source:{source}
dest:{dest}
type:{type}");
    }
}
