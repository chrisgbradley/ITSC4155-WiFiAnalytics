$(window).on('scroll', function () {
    var scroll = $(window).scrollTop();
    if (scroll >= 300) {
        $('.hero').removeClass('sticky-hero');
    } else {
        $('.hero').addClass('sticky-hero');
    }
})