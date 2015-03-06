
(function($) {
  "use strict";

    // Configs ! SET UP YOURS !
    var GOOGLE_MAP_ADDRESS = 'Mountain View, CA 94043',
        TWITTER_USERNAME   = 'Envato'; 


    // Layer Slider
    $('#layerslider').layerSlider({
        skinsPath : false,
        skin : false,
        thumbnailNavigation : 'hover',
        hoverPrevNext : false,
        responsive : false,
        responsiveUnder : 1070,
        sublayerContainer : 1070,
        showCircleTimer : false
    });


    // Global data-attributes
    $('*').filter(function(){
        if($(this).attr('data-bg-img') != undefined) {
            var bg_img_obj = $(this);
            bg_img_obj.addClass('bg-img');
            bg_img_obj.css('background-image', function(){
                return 'url(' + bg_img_obj.attr('data-bg-img') + ')';
            });
        }

        if($(this).attr('data-bg-pattern') != undefined) {
            var bg_pattern_obj = $(this);
            bg_pattern_obj.addClass('bg-pattern');
            bg_pattern_obj.css('background-image', function(){
                return 'url(' + bg_pattern_obj.attr('data-bg-pattern') + ')';
            });
        }

        if($(this).attr('data-bg-color') != undefined) {
            var bg_color_obj = $(this);
            bg_color_obj.css('background-color', bg_color_obj.attr('data-bg-color'));
        }

        if($(this).attr('data-bg-fade') != undefined) {
            var bg_fade_obj = $(this);
            bg_fade_obj.alphaBg({opacity : bg_fade_obj.attr('data-bg-fade')});
        }
    });


    // Bootstrap accordion
    $('.accordion').on('show', function (e) {
        $(e.target).prev('.accordion-heading').find('.accordion-toggle').addClass('active');
    });

    $('.accordion').on('hide', function (e) {
        $(e.target).prev('.accordion-heading').find('.accordion-toggle').removeClass('active');
    });

    $("#accordion-toggle .collapse").collapse();





    // Background Mask
    $('.bg-mask, .bg-mask-lighten, .bg-mask-darken, .bg-mask-color, .bg-mask-front').prepend('<div class="bg-mask-layer"></div>');



    // Background Slider
    $('#superslides').superslides({
        inherit_width_from: '.top-bg-area',
        inherit_height_from: '.top-bg-area',
        play: 3000,
        animation : 'fade'
    });


    $('#superslides-full').superslides({
        play: 3000,
        animation : 'fade'
    });



    // Countdown Timer
    $('#countdown').countdown();


    // Bootstrap Tooltips
    $('[data-toggle="tooltip"]').tooltip();


    // Lighbox gallery
    $('#popup-gallery').each(function () {
        $(this).magnificPopup({
            delegate: 'a.popup-gallery-image',
            type: 'image',
            gallery: {
                enabled: true
            }
        });
    });

    // Lighbox image
    $('.popup-image').magnificPopup({
        type: 'image'
    });

    // Lighbox text
    $('.popup-text').magnificPopup({
        removalDelay: 500,
        closeBtnInside: true,
        callbacks: {
            beforeOpen: function () {
                this.st.mainClass = this.st.el.attr('data-effect');
            }
        },
        midClick: true
    });

    // Lightbox iframe
    $('.popup-iframe').magnificPopup({
        dispableOn: 700,
        type: 'iframe',
        removalDelay: 160,
        mainClass: 'mfp-fade',
        preloader: false
    });



    $('.device-mockup > .device', '.contain').css("background-size", "contain");
    $('.cover').css("background-size", 'conver');

    // Self Hosted video plugin
    $('audio,video').mediaelementplayer();

    // Responsive videos
    $(document).ready(function () {
        $("body").fitVids();
    });


    $('#ri-grid').gridrotator({ animType : 'random'});
    $('#ri-grid-fade').gridrotator({ animType : 'fadeInOut'});
    $('#ri-grid-no-animation').gridrotator({slideshow : false});

    // Resize initiation for bunch of plugins
    // $(document).ready(function(){
    //     $(window).resize();
    // });

    // Css detection for centering position
    function pos(obj) {
        if($(obj).css('position') == 'static') {
            return 'relative';
        } else {
            return null;
        }
    }

    function zidx(obj) {
        if($(obj).css('z-index') == 'auto')  {
            return 10;
        } else {
            return null;
        }
    }    

    $(window).resize(function(){
        $('.to-top, .vertical-center').each(function(){
            $(this).css({
                position: pos(this),
                marginTop: -($(this).outerHeight()/2),
                zIndex: zidx(this)
            });
        });

        $('.to-top-half').each(function() {
            $(this).css({
                position: pos(this),
                marginTop: -($(this).outerHeight()/2),
                zIndex: zidx(this)
            });
        });

        $('.to-bottom').each(function(){
            $(this).css({
                position: pos(this),
                marginBottom: -($(this).outerHeight()/2),
                zIndex: zidx(this)
            });
        });

        $('.to-bottom-half').each(function(){
            $(this).css({
                position: pos(this),
                marginBottom: -($(this).outerHeight()/4),
                zIndex: zidx(this)
            });
        });
        // $(window).resize();




    // Caroufredsel content carousel
    // $(window).resize(function(){
        $('#fredsel').carouFredSel({
            auto: false,
            prev: '#fredsel-prev',
            next: '#fredsel-next'
        });

        $('#fredsel-2').carouFredSel({
            auto: false,
            prev: '#fredsel-2-prev',
            next: '#fredsel-2-next'
        });

         $('#fredsel-pagi').carouFredSel({
            auto: false,
            pagination: '#fredsel-pagi-pager',
        });

        if($(window).width() < 768) {
            $('#fredsel').children().css({
                width : $('#fredsel').parent().width(),
                float : 'left'
            });

             $('#fredsel-2').children().css({
                width : $('#fredsel-2').parent().width(),
                float : 'left'
            });

             $('#fredsel-pagi').children().css({
                width : $('#fredsel-pagi').parent().width(),
                float : 'left'
            });
        }


        $('#fredsel-hor-swipe').carouFredSel({
            responsive : true,
            width: '100%',
            prev: '#fredsel-hor-swipe-prev',
            next: '#fredsel-hor-swipe-next',
            scroll: 1,
            auto : false,
            items : {
                width : '100%',
                visible : {
                    max : 10,
                    min : 5
                }
            },
            swipe: {
                onMouse: true,
                onTouch: true
            }
        });

        if($(window).width() < 768) {
            $('#fredsel-hor-swipe').children().css({
                width : $('.fredsel-inline').width(),
                maxHeight : 600,
                margin : 0
            });
        }
    // });

    });

    // Main navigation
    $(function(){
        $('#flexnav').flexNav();
    });


    // One Page navigation
    $('#one-page-nav').onePageNav({
        filter: ':not(.external)',
        scrollThreshold: 0.25,
        currentClass: 'active',
    }); 

    // Google map
    $('#gmap').gmap3({
        marker:{
            address: GOOGLE_MAP_ADDRESS 
        },
        map:{
            options:{
            zoom: 14,
            scrollwheel: false,
            streetViewControl : false
            }
        }
    });


    // Twitter plugin
    $("#twitter").tweet({
        username: TWITTER_USERNAME, 
        count: 3
    });

    $("#twitter-ticker").tweet({
        username: TWITTER_USERNAME, 
        page: 1,
        count: 20
    });

    $(document).ready(function(){
    var ul = $('#twitter-ticker').find(".tweet-list");
      var ticker = function() {
        setTimeout(function() {
          ul.find('li:first').animate( {marginTop: '-4.7em'}, 850, function() {
            $(this).detach().appendTo(ul).removeAttr('style');
          });
          ticker();
        }, 5000);
      };
      ticker();
    });

    // Filter plugin similar to isotope
    $('#mixitup-grid').mixitup();

    // Sticky header
    $(document).ready(function() {
        if($('body').hasClass('sticky-header')) {
          var header = $('header.main'),  
              theLoc = header.position().top,
              shrinked = header.hasClass('shrink');         
          $(window).scroll(function() {
              if(theLoc >= $(window).scrollTop()) {
                  if(header.hasClass('fixed')) {
                      if(header.hasClass('shrink') && !shrinked) {
                        header.removeClass('shrink');
                      } 
                      header.removeClass('fixed');
                  }
              } else { 
                  if(!header.hasClass('fixed')) {
                      if(!header.hasClass('shrink')) {
                        header.addClass('shrink');
                      } 
                      header.addClass('fixed');
                  }
              }
          });
        }
    });



    // Show hided elements
    $(document).ready(function(){
        $('.show-onload').fadeIn(100);
        $(window).resize();
    });

})(jQuery);