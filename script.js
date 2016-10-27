//// Set active selected href
$(document).ready(function() {
    $("[href]").each(function() {
        if (this.href === window.location.href) {
            $(this).addClass("active");
        }
    });
});

$(document).ready(function() {
    $(".dropdown-menu li [href]").each(function () {
        if (this.href === window.location.href) {
            $(this).addClass("active");
            $(this).parent().parent().prev().addClass("active");
        }
    });
});