   M S B A M L   `   `   `  ���� A  =MyToDo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:MyToDo.Common.ViewsMyToDo.Common.Views  5clr-namespace:MyToDo.ExtensionsMyToDo.Extensions  #    MyToDo.Common.Views.LoginView   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     3local!clr-namespace:MyToDo.Common.Views     2passExclr-namespace:MyToDo.Extensions     _ [MaterialDesignThemes.Wpf, Version=4.4.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962a:md1http://materialdesigninxaml.net/winfx/xaml/themes  [ WPrism.DryIoc.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/   _	 [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors 	 $	��350��$	��600��#  Prism.Services.Dialogs.Dialog    WindowStyle  ��  �� 
TargetType" �.���  �� Property � �� �� Value$	 600����  � ��$	 350����  �@�
ResizeMode$ NoResize=���  �@�SizeToContent$ WidthAndHeight=���  �@ WindowStartupLocation$ CenterScreen=�.��� 
������� � ColumnDefinitions �� $
��1.3* ��� .+�'  !MaterialDesignThemes.Wpf.Snackbar  -     Name$ snackbar��$��2�� K� ZIndex$ 1��   MessageQueue 4  .MaterialDesignThemes.Wpf.MessageQueueExtension  �� $&�� /Images/computer_pc_PNG17492.png��7  1MaterialDesignThemes.Wpf.Transitions.Transitioner  $��1�� �� SelectedIndex �� *SelectedIndex+.@�<  6MaterialDesignThemes.Wpf.Transitions.TransitionerSlide  .���� $��1��$
��20 0q�$��Center=�$
��Left=�.+��� $��欢迎使用��$��5q�$��25(�$
��Bold"�$
��Left=��� $	��210��$��5q�)  #MaterialDesignThemes.Wpf.HintAssist	   Hint$	 请输入账号��.  (MaterialDesignThemes.Wpf.TextFieldAssist
   HasClearButton$

 True�� � Cursor$ Arrow������ *	Account+G� $	��210��$��5q�$	 请输入密码��$ Arrow��*   $MyToDo.Extensions.PasswordExtensions   Password �� *
Password+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�*	 	 $Microsoft.Xaml.Behaviors.Interaction 	  	Behaviors (
   "MyToDo.Extensions.PasswordBehavior
  �� $��5q�$��登录系统��$��Login������ *ExecuteCommand+]� $��False��.+��� $��注册账号��$	��5 0q�$
 Hand�� 	  Triggers + 	 %Microsoft.Xaml.Behaviors.EventTrigger     	EventName$ MouseLeftButtonDown��* 	 $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 2 	 ,Microsoft.Xaml.Behaviors.InvokeCommandAction     CommandParameter$ GoToRegister��   Command �� *ExecuteCommand+�� $��忘记密码?��$	��5 0q�$��Right=��� $��5 15 0 0q�
���  ������ *
ErrorMsg+  .���� $��1��$
��20 0q�$��Center=�$
��Left=�.+��� $��注册账号��$��5q�$��25(�$
��Bold"�$
��Left=��� $	��210��$��5q�$	 请输入账号��$

 True������ *	Account+�� $	��210��$��5q�$	 请输入名称��$

 True������ *
UserName+G� $	��210��$��5q�$	 请输入密码��$ Arrow�� �� *
Password+$ TwoWay=�$ PropertyChanged=� 
  G� $	��210��$��5q�$	 请再次输入密码��$ Arrow�� �� *
RePassword+$ TwoWay=�$ PropertyChanged=� 
  ]� $��False��.+��� $��5q�$��注册账号��$��Register������ *ExecuteCommand+�� $��5q�$��Right=�$��返回登录��$��Return������ *ExecuteCommand+�� $��5 15 0 0q�
���  ������ *
ErrorMsg+