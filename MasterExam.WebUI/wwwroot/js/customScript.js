let whiteContent = localStorage.getItem("white-content");
let new_color = localStorage.getItem("newColor");
let sidebarMini = localStorage.getItem("sidebarMini");

$sidebar = $('.sidebar');
$navbar = $('.navbar');
$main_panel = $('.main-panel');
$full_page = $('.full-page');
$sidebar_responsive = $('body > .navbar-collapse');

$sidebar.attr('data', new_color);
$main_panel.attr('data', new_color);
$full_page.attr('filter-color', new_color);
$sidebar_responsive.attr('data', new_color);

$('body').removeClass();
$('body').addClass(whiteContent);
$('body').addClass(sidebarMini);
