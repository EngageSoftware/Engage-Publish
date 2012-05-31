/*globals jQuery */
(function ($) {
    'use strict';
    $('.news_item_con, .news_item_con_over')
        .live('mouseenter', function () {
            $(this).addClass('news_item_con_over').removeClass('news_item_con');
        }).live('mouseleave', function () {
            $(this).addClass('news_item_con').removeClass('news_item_con_over');
        }).live('click', function (event) {
            window.location = $('.title', this).attr('href');
            event.preventDefault();
        });
}(jQuery));