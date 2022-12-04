// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/* When the user clicks on the button,
toggle between hiding and showing the dropdown content */
function navDropdownListToggle() {
    $('.dropdown-content')[0].classList.toggle("u-show");
}

// Close the dropdown menu if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('.dropdown-item')) {

        $('.dropdown-content').each(() => { $(this).removeClass("u-show"); })
    }
}

// Get the header
$(window).on('scroll', function () {
    if ($(window).scrollTop()) {
        $('header').addClass('sticky-header');
    } else {
        $('header').removeClass('sticky-header');
    }
})