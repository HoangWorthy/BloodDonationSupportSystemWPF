   M S B A M L   `   `   `  ����!  BloodDonationSupportSystemWPF1    +BloodDonationSupportSystemWPF.AddBlogWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Thêm Blog Mới��5   	   $	��600��6   $	��500��6,    � WindowStartupLocation$ CenterScreen=�5   	   ���#F5F5F5.��5      ��  �� CornerRadius$ 8��6    �� Padding$ 20q�6   
����������#DDD$��1q�5       $��20q�64   ._�5	   
   �  � RowDefinitions 5
      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $��* �6    �� 5      $
��Auto �6     � ColumnDefinitions 5      �� 5      $	��120 �6#   �� 5      $��* �6#   .+�5      �� $��Tiêu đề:��6   $��0��6(   $��0��65   $��Center=�5      $��
0,10,10,10q�63   �� 5      -   5         ��Name$ txtTitle��5      $��0��6(   $��1��65   $��30��5      $��5q�6"   $��	0,10,0,10q�6.   �� 5      $��Nội dung:��6   $��1��6)   $��0��66   $	��Top=�5       $��
0,10,10,10q�60   �� 5!      -   5        $ 
txtContent��5!      $��1��6*   $��1��67   $	��150��5"      $��5q�6#   $��	0,10,0,10q�6/    �� AcceptsReturn$
 True��5#       �� TextWrapping$
 Wrap=�6+   "	 �� VerticalScrollBarVisibility$
	 Auto=�5$      �� 5'      $��Tác giả:��6   $��2��6(   $��0��65   $��Center=�5(      $��
0,10,10,10q�63   �� 5)      -   5        
 ��Name$
 cbAuthor��5)      $��2��6)   $��1��66   $��30��5*      $��	0,10,0,10q�6#    �� DisplayMemberPath$ Email��5+       �� SelectedValuePath$ Id��5,      �� 5/      $��Ngày đăng:��6   $��3��6*   $��0��67   $��Center=�50      $��
0,10,10,10q�63   (  "System.Windows.Controls.DatePicker  51      -   5          Name$ 
datePicker��51      $��3��6-   $��1��6:   $��30��52      $��	0,10,0,10q�6%   �� 55      $��5��6   $��1��6&    �� Orientation$ 
Horizontal=�66   $��Right=�56      .+�57      �� -   5        $
��Lưu��57      $	��100��6'   $��35��63   ���#4CAF50
�������$��0q�59      $��0,20,0,0q�6-   5:      5;      5<      5=      5>      