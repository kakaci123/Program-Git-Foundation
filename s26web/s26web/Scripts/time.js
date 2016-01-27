jQuery(document).ready(function () {
    utc_to_time("utc_time_temp", false);
    utc_to_time("utc_time_temp_editor", true);
});
function utc_to_time(cls, edit) {
    $("." + cls).each(function () {
        var d = new Date();
        var n = d.getTimezoneOffset() * (-1);
        var value = edit ? $(this).val() : $(this).html();
        var format = $(this).attr("data-format");
        if (format == null || format == '' || format == undefined)
        {
            format = "yyyy/MM/dd HH:mm:ss";
        }
        if (value != null && value != "") {
            var time = new Date(value);
            time.setMinutes(time.getMinutes() + n);
            if (edit)
            { $(this).val(time.toString(format)); }
            else
            {
                $(this).parent().append(time.toString(format));
                $(this).remove();
            }
            //$(this).removeClass(cls);
            //$(this).removeAttr("class");
        }
        else {
            if (edit)
            { $(this).val(""); }
            else
            {
                $(this).parent().append("NULL");
                $(this).remove();
            }
        }
    });
}