// See https://aka.ms/new-console-template for more information
using NSModel;
using NSViewModel;
using NSViews;

M_Model M = new M_Model();
M.Set_language("fr"); //test, the value will be read from the json file
VM_ViewModel VM = new VM_ViewModel(M);
V_Menu menu = new V_Menu(VM);

