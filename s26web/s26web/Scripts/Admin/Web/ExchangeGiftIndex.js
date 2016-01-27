$(function () {
    $("#search_time_offset").val(new Date().getTimezoneOffset());
    $(".date_search").datetimepicker({
        //showOn: 'both', showOtherMonths: true,
        //showWeeks: true, firstDay: 7, changeFirstDay: false,
        //buttonImageOnly: true, buttonImage: 'calendar.gif'
        altFormat: "yyyy/mm/dd",
        timeFormat: "HH:mm:ss",
        hourGrid: 4,
        minuteGrid: 10,
        //stepMinute: 30,
        //showSecond: false,
        onClose: function (dateText, inst) {
            if (dateText != "" && dateText != null)
            { $(this).val(dateText); }
        },
        onSelect: function (e, t) {
            if (!t.clicks) t.clicks = 0;

            if (++t.clicks === 2) {
                t.inline = false;
                t.clicks = 0;
            }
            setTimeout(function () {
                t.clicks = 0
            }, 500);
        }
    });
});
