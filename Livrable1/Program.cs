// See https://aka.ms/new-console-template for more information

using NSModel;
using NSViewModel;
using NSViews;
using System;

class Program
{
    public static void Main(string[] args)
    {
        M_Model M = new M_Model();
        VM_ViewModel VM = new VM_ViewModel(M);
        V_Menu menu = new V_Menu(VM);
    }
}


