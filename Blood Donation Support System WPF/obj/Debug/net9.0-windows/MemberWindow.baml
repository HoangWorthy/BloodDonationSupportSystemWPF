   M S B A M L   `   `   `  ����X  TBloodDonationSupportSystemWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:Blood_Donation_Support_System_WPF!Blood_Donation_Support_System_WPF  5   	   4    .Blood_Donation_Support_System_WPF.MemberWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Alocal/clr-namespace:Blood_Donation_Support_System_WPF     5   	     � Title$+  %Blood Donation System - Member Portal��5   	   $	��700��67   $
��1000��6D    � WindowStartupLocation$ CenterScreen=�5	   	    � WindowState$ 	Maximized=�6.   .��5
      �  � RowDefinitions 5   
   �� 5      $��70 �6   �� 5      $��* �6   .+�5   
   �� $��0��6   ���#D32F2F._�5      �  � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $	��200 �6'   .+�5      �� $��0��6    �� Orientation$ 
Horizontal=�6-   $��Center=�6F   $
��20,0q�6a   .+�5      �� $
��🩸��6    $��24(�6*   $��0,0,10,0q�68   �� 5      $$��Blood Donation Portal - Member��6    $��20(�5      $
��Bold"�6-   
�������$��Center=�6R   5      �� 5      $��1��6   $ 
Horizontal=�6-   $��Right=�6F   $��Center=�5      $��0,0,20,0q�67   .+�5       �� -   5        $��WelcomeText��5        $��Chào mừng, Member!��63   
�������$��SemiBold"�5!   2   $��Center=�6H   $��0,0,15,0q�6c   �� 5"      -   5        $��LogoutButton��5"      $��Đăng xuất��61   ���#B71C1C
�������$��0q�5#   /   $
��15,8q�6C    � Cursor$
 Hand��6R   5$      5%      5&      �� 5)   
   $��1��6   
������� � 	Resources 5*       �� 
TargetType	 �� Property
 �� Value��%�   '��       �� 5+      " ��6   .�5,      �� 	 � ��$
 14(�61   �� 5-      	 � ��$
 SemiBold"�63   �� 5.      	 � ��$
 20,12q�60   5/      50      .@�53      �� $��🏠 Trang chủ��6   .��54      �� $
��Auto=�6   .��55      � $��30q�6    56      �� 57      $
��Auto �6,   �� 58      $
��Auto �6,   �� 59      $��* �6,   .+�5=      �� $��0��6!   ���#FFEBEE �� CornerRadius$ 10��6C    �� Padding$ 25q�6U   $��0,0,0,20q�6b   ._�5>      �� .+�5?   "   �� $9��3Chào mừng đến với hệ thống hiến máu!��6,   $��24(�5@   +   $
��Bold"�69   ���#D32F2F$��0,0,0,10q�6`   �� 5A   "   $����Cảm ơn bạn đã tham gia vào sứ mệnh cứu người bằng việc hiến máu. Mỗi giọt máu của bạn đều có thể cứu sống một sinh mệnh.��6,   $��16(�5B   +   $
��Wrap=�69   ���#4242426M   5C      5D      A� 5G      $��1��6&    A� Columns$ 3��63   $��0,0,0,30q�6?   .+�5H      �� ���#E8F5E8$ 10��6:   $ 20q�6L   $��0,0,10,0q�6Y   ._�5I   "   �� $��Center=�6-   .+�5J   &   �� $
��🎯��60   $��30(�6:   $��Center=�6H   �� 5K   &   -   5        $��EventCountText��5K   0   $��5��6F   $��24(�6O   $
��Bold"�6]   $��Center=�5L   /   ���#4CAF506L   �� 5M   &   $��Sự kiện sắp tới��60   $��14(�6G   $��Center=�6U   ���#6665N   /   5O   #   5P      �� 5R      ���#FFF3E0$ 10��6:   $ 20q�6L   $	��5,0q�6Y   ._�5S   "   �� $��Center=�6-   .+�5T   &   �� $
��🚨��60   $��30(�6:   $��Center=�6H   �� 5U   &   -   5        $��UrgentRequestText��5U   0   $��3��6I   $��24(�6R   $
��Bold"�6`   $��Center=�5V   /   ���#FF98006L   �� 5W   &   $��Yêu cầu khẩn cấp��60   $��14(�6H   $��Center=�6V   ���#6665X   /   5Y   #   5Z      �� 5\      ���#E3F2FD$ 10��6:   $ 20q�6L   $��10,0,0,0q�6Y   ._�5]   "   �� $��Center=�6-   .+�5^   &   �� $
��📰��60   $��30(�6:   $��Center=�6H   �� 5_   &   -   5        $��NewBlogText��5_   0   $��8��6C   $��24(�6L   $
��Bold"�6Z   $��Center=�5`   /   ���#2196F36L   �� 5a   &   $��Bài viết mới��60   $��14(�6D   $��Center=�6R   ���#6665b   /   5c   #   5d      5e      � 5h      $��2��6    5i      �� 5j   "   $��* �63   �� 5k   "   $��* �63   .+�5o      �� $��0��6%   
����������#E0E0E0$��1q�5p   $   $ 10��68   $ 20q�6J   $��0,0,15,0q�6W   ._�5q   "   �� .+�5r   &   �� $)��#Sự kiện hiến máu gần đây��60   $��18(�6P   $
��Bold"�6^   ���#D32F2F$��0,0,0,15q�5s   D   �� 5t   &   -   5        $��RecentEventsListView��5t   /   $	��200��6K   $��0q�6X   .@�5u   *   �� .��5v   .   �� .+�5w   2   �� $1��+Hiến máu tại Bệnh viện Chợ Rẫy��6<   $��SemiBold"�6b   �� 5x   2   $��25/07/2025 - TP.HCM��6<   ���#666$��12(�6i   5y   /   5z   +   �� 5{   *   .��5|   .   �� .+�5}   2   �� $0��*Chương trình hiến máu cộng đồng��6<   $��SemiBold"�6c   �� 5~   2   $��28/07/2025 - Hà Nội��6<   ���#666$��12(�6i   5   /   5�   +   5�   '   5�   #   5�      �� 5�      $��1��6%   
����������#E0E0E0$��1q�5�   $   $ 10��68   $ 20q�6J   $��15,0,0,0q�6W   ._�5�   "   �� .+�5�   &   �� $"��Yêu cầu máu khẩn cấp��60   $��18(�6L   $
��Bold"�6Z   ���#FF5722$��0,0,0,15q�5�   D   �� 5�   &   -   5        $��UrgentRequestsListView��5�   /   $	��200��6M   $��0q�6Z   .@�5�   *   �� .��5�   .   �� .+�5�   2   �� $(��"Cần nhóm máu O- (KHẨN CẤP)��6<   $��SemiBold"�6^   ���#D32F2F6t   �� 5�   2   $#��Bệnh viện 108 - Hà Nội��6<   ���#666$��12(�6l   5�   /   5�   +   �� 5�   *   .��5�   .   �� .+�5�   2   �� $)��#Cần nhóm máu AB+ (KHẨN CẤP)��6<   $��SemiBold"�6_   ���#D32F2F6u   �� 5�   2   $*��$Bệnh viện Đại học Y Dược��6<   ���#666$��12(�6n   5�   /   5�   +   5�   '   5�   #   5�      5�      5�      5�      5�      �� 5�      $!��📅 Đăng ký hiến máu��6   .��5�      � $��20q�6    5�      �� 5�      $
��Auto �6(   �� 5�      $��* �6(   .+�5�      �� $��0��6   ���#F5F5F5$ 5��6?   $ 15q�6P   $��0,0,0,20q�6]   ._�5�      �  5�      �� 5�   "   $��* �63   �� 5�   "   $	��200 �63   �� 5�   "   $
��Auto �63   .+�5�      �� -   5        $��EventSearchTextBox��5�   &   $��0��6@   $��35��6P   $��Center=�5�   %   $
��10,0q�6G   $ ��Tìm kiếm sự kiện...��5�   %   ���#999���#DDD$��0,0,10,0q�5�   8   �� 5�      -	   5        $��CityFilterComboBox��5�   '   $��1��6A   $��35��6Q   $��0,0,10,0q�5�   &   $��Center=�68   .@�5�   "   �� $��Tất cả thành phố��6/    �� 
IsSelected$
 True��6J   �� 5�   "   $��	Hà Nội��6/   �� 5�   "   $��TP. Hồ Chí Minh��6/   �� 5�   "   $��Đà Nẵng��6/   5�      �� 5�      -
   5        $��SearchButton��5�   %   $��2��69   $��Tìm kiếm��6I   $��35��5�   $   $
��20,0q�60   ���#D32F2F
�������$��0q�5�   7   5�      5�      �� 5�      -   5        $��EventsListView��5�      $��1��65   $��Disabled=�6B   ��5�      �� 5�      .R�5�   "   �� 
����������#E0E0E0$��1q�6R   $ 10��5�   (   $	��0,5q�6:   $ 20q�6G   ._�5�   &   �  5�   *   �� 5�   .   $��* �6?   �� 5�   .   $
��Auto �6?   .+�5�   *   �� $��0��65   .+�5�   .   �� $��16(�5�   7   $
��Bold"�6E   $��0,0,0,5q�6W   ��5�   8   �� *Name+5�   W   �� 5�   .   ���#666$��0,0,0,5q�6J   �� 5�   2   $
�📍��66    �� 5�   2   �66   �� *
Location+5�   /   �� 5�   .   ���#666$��0,0,0,5q�6J   �� 5�   2   $
�📅��66    �� 5�   2   �66   �� *Date+ �� 5�   2   $	� - ��66    �� 5�   2   �66   �� *	TimeRange+5�   /   �� 5�   .   ���#4CAF50$��SemiBold"�6M   �� 5�   2   $�👥 Đã đăng ký: ��66    �� 5�   2   �66   �� *RegistrationCount+ �� 5�   2   $�	 người��66   5�   /   5�   +   �� 5�   *   $��1��65   $��Center=�6E   .+�5�   .   �� -   5        $��
Đăng ký��5�   5   ���#4CAF50
�������$��0q�5�   4   $��20,10q�6H   y�5�   4   �� *CanRegister+D   >Blood_Donation_Support_System_WPF.BooleanToVisibilityConverter   Instance �� 	Converter# Z �� 5�   .   $��Đã đăng ký��65   ���#9E9E9E
�������$��0q�5�   4   $��20,10q�6H   ${�False��5�   4   y�5�   4   �� *IsUserRegistered+# Z �� 5�   .   $��Đã đầy��65   ���#F44336
�������$��0q�5�   4   $��20,10q�6H   ${�False��5�   4   y�5�   4   �� *IsFull+# Z �� 5�   .   -   5        $��
Chi tiết��5�   5   ���#2196F3
�������$��0q�5�   4   $��20,10q�6H   $��0,5,0,0q�5�   4   5�   4   5�   +   5�   '   5   #   5     5     5     5     �� 5     $��	📰 Blog��6   .��5	     � $��20q�6    5
     �� 5     $
��Auto �6(   �� 5     $��* �6(   .+�5     �� $��0��6    $0��*Tin tức và bài viết về hiến máu��6-   $��20(�5     $
��Bold"�6-   ���#D32F2F$��0,0,0,20q�6T   �� 5     -   5        $��BlogListView��5     $��1��63   ��5     �� 5     .R�5  "   �� 
����������#E0E0E0$��1q�6R   $ 10��5  (   $
��0,10q�6:   $ 20q�6H   ._�5  &   �  5  *   �� 5  .   $	��120 �6?   �� 5  .   $��* �6?   .+�5  *   �� $��0��61   ���#F0F0F0$ 5��6V   $��80��5  0   $	��100��6<   $��0,0,15,0q�6H   ._�5  .   �� $
��📷��68   $��30(�6B   $��Center=�6P   $��Center=�5   7   ���#9996R   5!  +   �� 5#  *   $��1��65   .+�5$  .   �� $:��4Lợi ích của việc hiến máu thường xuyên��68   $��16(�5%  7   $
��Bold"�6E   $��0,0,0,8q�6W   �� 5&  .   $����Hiến máu không chỉ giúp cứu sống người khác mà còn mang lại nhiều lợi ích cho sức khỏe của bạn...��68   $
��Wrap=�5'  7   ���#666$��0,0,0,8q�6]   �� 5(  .   $L��F📅 Ngày đăng: 24/07/2025 | ✍️ Tác giả: BS. Nguyễn Văn A��68   $��12(�5)  7   ���#9996E   5*  +   5+  '   5,  #   5-     5/     50     51     �� 54     $'��!🩸 Yêu cầu máu khẩn cấp��6   .��55     � $��20q�6    56     �� 57     $
��Auto �6(   �� 58     $��* �6(   .+�5;     �� $��0��6   ���#FFEBEE$ 5��6?   $ 15q�6P   $��0,0,0,20q�6]   ._�5<     �� .+�5=     �� $)��#⚠️ YÊU CẦU MÁU KHẨN CẤP��6(   $��18(�6G   $
��Bold"�6U   ���#D32F2F$��Center=�5>  <   �� 5?     $���{Dưới đây là danh sách các yêu cầu máu có mức độ ưu tiên cao cần được hỗ trợ ngay lập tức.��6(   $��Center=�5@  '   ���#666$��0,5,0,0q�6V   5A     5B     �� 5D     -   5        $ ��UrgentBloodRequestListView��5D     $��1��6A   ��5E     �� 5F     .R�5G  "   �� ���#FFF���#FFCDD2$��2q�6Q   $ 10��5H  (   $
��0,10q�6:   $ 20q�6H   ._�5I  &   �  5J  *   �� 5K  .   $��* �6?   �� 5L  .   $
��Auto �6?   .+�5O  *   �� $��0��65   .+�5P  .   �� $-��'🚨 KHẨN CẤP - Cần nhóm máu O-��68   $��16(�5Q  7   $
��Bold"�6E   ���#D32F2F$��0,0,0,10q�6l   �� 5R  .   $3��-🏥 Bệnh viện: Bệnh viện Chợ Rẫy��68   $��0,0,0,5q�6_   �� 5S  .   $>��8📍 Địa chỉ: 201B Nguyễn Chí Thanh, Q.5, TP.HCM��68   $��0,0,0,5q�6n   �� 5T  .   $,��&🩸 Số lượng cần: 3 đơn vị��68   $��0,0,0,5q�6Y   �� 5U  .   $-��'⏰ Thời gian: Cần ngay lập tức��68   ���#FF5722$��SemiBold"�6r   5V  +   �� 5X  *   $��1��65   $��Center=�6E   .+�5Y  .   �� ���#D32F2F$ 20��6J   $
 15,8q�6\   $��0,0,0,10q�6k   ._�5Z  2   �� $��KHẨN CẤP��6<   
�������$
��Bold"�6_   $��12(�6q   5[  /   �� 5\  .   $��Liên hệ hỗ trợ��65   ���#4CAF50
�������$��0q�5]  G   $
��15,8q�6[   5^  +   5_  '   5`  #   5a     5c     5d     5e     5f     5g     5h     