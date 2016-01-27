
Modernizr.load([
        {
            test: Modernizr.inputtypes && Modernizr.inputtypes.date,
            nope: '/Scripts/jquery-ui-1.8.17.min.js',
            complete: function() {
                $(function() {
                    $("input.date-picker").datepicker();
                });
            }
        },
        {
            test: Modernizr.input && Modernizr.input.placeholder,
            nope: ['/Scripts/jquery.placeholder.min.js'],
            complete: function() {
                $(function() {
                    $('input').placeholder();
                });
            }
        }
    ]);