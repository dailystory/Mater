/*
┌───────────────────────────────────────────────────────────────────┐
│ Mater | version 1.0.1 | Aug 2017					                │
├───────────────────────────────────────────────────────────────────┤
│ Learn more: https://github.com/dailystory/Mater                   │
│ Maintained by: DailyStory - https://dailystory.com/               │
└───────────────────────────────────────────────────────────────────┘
*/
var Mater = Mater || {

    // path to edit on GithUB
    editPath: ''
}

Mater.Article = {

    /*
    Initializes or re-initializes all of the objects, these
    needs to be done since we are dynamically loading in
    content pages.
    */
    init: function() {
        // Set the height
        setTocHeight();

        // reset suggest edits
        $('#suggestEdits').prop('href', Mater.editPath);

        // reset code highlights
        SyntaxHighlighter.highlight();

        // reset the links
        anchors.add('#bodyContent h2, #bodyContent h3, #bodyContent h4'); // add anchor link UX


        /*
        Popstate handler for managing the browser
        history state
        */
        window.onpopstate = function (e) {
            if (e.state) {
                $('#bodyContent').html(e.state.html);
                document.title = e.state.pageTitle;
            }
        };


        /*
        Set the height of the table of contents based on the
        content height or viewport height
        */
        function setTocHeight() {

            // set the scroll height
            var contentHeight = $("#bodyContent").height();
            var viewportHeight = window.innerHeight;

            if (contentHeight > viewportHeight) {
                $('.tocContent').css({ 'height': ($('#bodyContent').height() + 'px') });
            } else {
                $('.tocContent').css({ 'height': (viewportHeight + 'px') });
            }
        }
    },

    /*
    Called in the article class when the document is ready
    */
    ready: function () {

        // initialize
        this.init();

        /*
        Handle clicks on the table of contents to load the
        requested content in the display window dynamically
        and not request a full load of all the UX. This keeps
        the table of content scrolled to the current location
        */
        $('.tocContent a').click(function (e) {

            e.preventDefault();

            document.title = this.text;
            var href = this.href;

            // Load in the requested body content and manage the history
            $('#bodyContent').load(this.href + ' #bodyContent>*', function () {
                $('#bodyContent').fadeIn('fast');
                window.history.pushState({ 'html': $('#bodyContent').html(), 'pageTitle': document.title }, '', href);

                // reset suggest edits
                $('#suggestEdits').prop('href', Mater.editPath);

                // re-init
                Mater.Article.init();

            });

        });

    },

}