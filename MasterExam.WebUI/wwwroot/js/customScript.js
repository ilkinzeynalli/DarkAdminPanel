let backGroundColor = localStorage.getItem("backGroundColor");
let sidebarColor = localStorage.getItem("sidebarColor");

$('body').addClass(backGroundColor);
$(".sidebar").attr('data', sidebarColor);
$(".sidebar").attr('data-color', sidebarColor);
$("div.main-panel.ps").attr('data', sidebarColor);

$("span.light-badge").on("click", function () {
    localStorage.setItem("backGroundColor", "white-content");
});

$("span.dark-badge").on("click", function () {
    localStorage.setItem("backGroundColor", "");
});

$("span.badge.filter.badge-primary").on("click", function () {
    localStorage.setItem("sidebarColor", "primary");
});

$("span.badge.filter.badge-info").on("click", function () {
    localStorage.setItem("sidebarColor", "blue");
});

$("span.badge.filter.badge-success").on("click", function () {
    localStorage.setItem("sidebarColor", "green");
});

$("span.badge.filter.badge-warning").on("click", function () {
    localStorage.setItem("sidebarColor", "orange");
});

$("span.badge.filter.badge-danger").on("click", function () {
    localStorage.setItem("sidebarColor", "red");
});

$('#changeTemp').on('switchChange.bootstrapSwitch', function (event, state) {
    if (state) 
        localStorage.setItem("backGroundColor", "");
    else 
        localStorage.setItem("backGroundColor", "white-content");
});
