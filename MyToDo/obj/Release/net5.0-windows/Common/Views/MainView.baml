   M S B A M L   `   `   `  ���� A  =MyToDo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:MyToDoMyToDo  5clr-namespace:MyToDo.ExtensionsMyToDo.Extensions  "    MyToDo.Common.Views.MainView   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors   localclr-namespace:MyToDo  )extclr-namespace:MyToDo.Extensions  _ [MaterialDesignThemes.Wpf, Version=4.4.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  [ WPrism.DryIoc.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59T	 PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/  	 ! 	 Prism.Mvvm.ViewModelLocator    AutoWireViewModel  True   MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�   TextRenderingMode$
 Auto=�   MaterialDesignDarkBackground#���  $��微软雅黑)� � WindowStyle$
 None=� � WindowStartupLocation$ CenterScreen=� � AllowsTransparency$
 True�� � Title$ 
MainWindow��$	��768��$
��1280��.���  � RowDefinitions �� $
��133* ��� $
��152* ��� $
��483* �.+�)  #MaterialDesignThemes.Wpf.DialogHost  -     Name$ 
DialogHost��	   
Identifier$
	 Root��
   DialogTheme$
 Inherit=�$��3��.��)  #MaterialDesignThemes.Wpf.DrawerHost  -     Name$ 
drawerHost��   IsLeftDrawerOpen ��  �� ElementName$ MenuToggleButton�� �� Path$ 	IsChecked�   LeftDrawerContent ]� $	��220��#���  .+��� $	��Top=�$
��0 20q�.+��� $��/Images/user.jpeg��$��60��$��60�� J� Clip @�  @� RadiusX$ 30Q� @� RadiusY$ 30Q� @� Center$ 30,30&��� $��15(�
�������$��Center=�$
��0 10q����� *
UserName+�� -    ��Name$ ListBoxMenuBar��  MyListBoxItemStyle#��[ ���� *
Menubars+*  $Microsoft.Xaml.Behaviors.Interaction   Triggers +  %Microsoft.Xaml.Behaviors.EventTrigger     	EventName$ SelectionChanged��*  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command �� *NavigateCommand+   CommandParameter �� $ ListBoxMenuBar��$ SelectedItem����� .R���  �� Orientation$ 
Horizontal=�
������ .+�'	  !MaterialDesignThemes.Wpf.PackIcon	  $
��15 0q� 	  Kind �� *Icon+�� $
��10 0q����� *Title+.���  �� $
��auto ��� .+�'
  !MaterialDesignThemes.Wpf.Snackbar
  -    
 Name$ snackbar�� K� ZIndex$ 1�� 
  MessageQueue 4  .MaterialDesignThemes.Wpf.MessageQueueExtension  (  "MaterialDesignThemes.Wpf.ColorZone  -     Name$ colorZoneNavigate��$��16q�+  %MaterialDesignThemes.Wpf.ShadowAssist    ShadowDepth$  Depth2=�!   Mode$! 
PrimaryMid=�$	��Top=�.��]� $
��True��.+��� $ 
Horizontal=�$��Right=�.+�'  !MaterialDesignThemes.Wpf.PopupBox  "   ToggleContent" �� $��/Images/user.jpeg��$��30��$��30��$��0 0 15 0q� @� $ 15Q�$ 15Q�$ 15,15&�#   PopupContent.# �� .+��� $��注销账号������ *LogoutCommand+�� -   $ ��Name$$ btnMin��$	��－��$��宋体)� ! MaterialDesignFlatMidBgButton#��[ �� -   $$ btnMax��$	��☐��$��宋体)�#��[ �� -   $$ btnClose��$	��✕��$��宋体)�#��[ �� $ 
Horizontal=�.+�d� -	   % d�Name$% MenuToggleButton�� ' #MaterialDesignHamburgerToggleButton#��[ & d� 	IsChecked$& FalseY�4  .System.Windows.Automation.AutomationProperties'   Name$' HamburgerToggleButton���� $��24,0,0,0q�( � ToolTip$( Previous Item��  MaterialDesignToolButton#��[ ) � Cursor$
) Hand������ *GoBackCommand+��0  *MaterialDesignThemes.Wpf.PackIconExtension  *   Kind$* 	ArrowLeft=�+   Size+ 24���� , �� RelativeSource, �� - �� AncestorType"- �$ (TextElement.Foreground)�+  %MaterialDesignThemes.Wpf.RippleAssist.   Feedback. �� , �� *Self+$ 
Foreground�  BrushRoundConverter/ �� 	Converter#/ [ �� $��16,0,0,0q�$( 	Next Item��#��[ ���� *GoForwardCommand+��  $* 
ArrowRight=�+ 24���� , �� "- �$ (TextElement.Foreground)�. �� , �� *Self+$ 
Foreground�#/ [ �� $��Center=�$��22(�$��50,0,0,0q�$%' Material Design In XAML Toolkit��$��	笔记本���� $��1��$   MyToDo.Extensions.PrismManager0   MainViewRegionName! 	 Prism.Regions.RegionManager1   
RegionName#1 Z0 