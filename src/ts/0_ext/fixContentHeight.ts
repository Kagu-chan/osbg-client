/**
 * Module function to fix the dynamic height of scrollable content
 */
module ContentWindowFix {
    export function run() {
        var headerHeight = jQuery("header").height();
        var content = jQuery("section.container");
        var x:number = content.offset().top
        if (x < headerHeight) {
            content.css('top', headerHeight);
            content.css('bottom', headerHeight);
        }
        if (x > headerHeight) {
            content.css('top', headerHeight);
            content.css('bottom', headerHeight);
        }
    }
}
ContentWindowFix.run();
jQuery(window).resize(ContentWindowFix.run);