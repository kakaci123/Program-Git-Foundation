$(function () {
    /*
    $("select[name=year],select[name=month]").change(function () {
        var y = parseInt($("select[name=year] option:selected").val(), 10);
        var m = parseInt($("select[name=month] option:selected").val(), 10);

        if ($("select[name=date]").attr("data-val") != undefined) {
            date = Get_Number(1, Get_Days(y, m), parseInt($("select[name=date]").attr("data-val"), 10));
            $("select[name=date]").html(date);
            $("select[name=date]").removeAttr("data-val");
        }
        else {
            date = Get_Number(1, Get_Days(y, m), -1);
            $("select[name=date]").html(date);
        }
    });

    var d = new Date();
    var year = Get_Number(2000, d.getFullYear(), 
        $("select[name=year]").attr("data-val") != undefined ?
            parseInt($("select[name=year]").attr("data-val"), 10) :-1);
    $("select[name=year]").html(year);
    if ($("select[name=year]").attr("data-val") != undefined)
    {
        $("select[name=year]").removeAttr("data-val");
    }

    var month = Get_Number(1, 12,
        $("select[name=month]").attr("data-val") != undefined ?
            parseInt($("select[name=month]").attr("data-val"), 10) : -1);
    $("select[name=month]").html(month);
    if ($("select[name=month]").attr("data-val") != undefined) {
        $("select[name=month]").removeAttr("data-val");
    }
    $("select[name=year]").trigger('change');
});
*/
    
    var d = new Date();
    var amount = 0;
    var year = d.getFullYear();
    var y = parseInt($("select[name=year] option:selected").val(), 10);
    var m = parseInt($("select[name=month] option:selected").val(), 10);
    var monthNow = d.getMonth();
    //year
    $(".default[name=year][data-count='0']").append(Get_Number(year - 7, year + 1, false));
    //month
    $(".default[name=month][data-count='0']").append(Get_Number(1, 12, false));
    //day
    $(".default[name=date][data-count='0']").append(Get_Number(1, Get_Days(y, m), false));
    
    for (var i = 0; i <= amount; i++) {
        $(".default[name=year][data-count='0']").change(function () {
            $(this).next().empty();
            $(this).next().append(Get_Number(1, 12, false));
            $(this).next().next().empty();
            $(this).next().next().append(Get_Number(1, Get_Days(y, m), false));
        });
        $(".default[name=month][data-count='0']").change(function () {
            y = parseInt($(this).prev().val(), 10);
            m = parseInt($(this).val(), 10);
            $(this).next().empty();
            $(this).next().append(Get_Number(1, Get_Days(y, m), false));
        });
    }


    //onchange
    
$("#addBaby").click(function () {
        addcount = parseInt(amount, 10) + 2;
        $("#baby_div").append("<div class=\"editor-label\">寶寶" + addcount + "生日  </div>").append("<div class=\"editor-field\"><select name=\"year\" data-count=" + addcount + " class=\"default\" data-val=\"\" tabindex=\"\"></select><select name=\"month\" data-count=" + addcount + " class=\"default\" data-val=\"\" tabindex=\"\"></select><select name=\"date\" data-count=" + addcount + " class=\"default\" data-val=\"\" tabindex=\"\"></select></div>");
        //var paddingTop = parseInt($(this).css("padding-top"), 10);
        $(".default[name=year][data-count='" + addcount + "']").append(Get_Number(year - 7, year + 1, false));
        $(".default[name=month][data-count='" + addcount + "']").append(Get_Number(1, 12, false));
        $(".default[name=date][data-count='" + addcount + "']").append(Get_Number(1, Get_Days(y, m), false));
        Empty(addcount);
        amount++;
});

});
function Get_Number(start,end,select)
{
    var selected = "selected=\"selected\"";
    var html = "<option value=\"\" " + (select==0 ? selected :"") + ">請選擇</option>";
    for (var i = start; i <= end; i++) {
        html += "<option value=\"" + i + "\" " + (select == i ? selected : "") + ">" + i + "</option>";
    }
    return html;
}
function Get_Days(y, m)
{
    if (y % 4 == 0 && m == 2)
    {
        return 29;
    }
    if (m == 2) {
        return 28;
    }
    if (m == 1 || m == 3 || m == 5 || m == 7 || m == 8 || m == 10 || m == 12)
    {
        return 31;
    }
    if (m == 4 || m == 6 || m == 9 || m == 11) {
        return 30;
    }
    return 0;
}
function Empty(Num) {
    $(".default[name=year][data-count='" + Num + "']").change(function () {
        $(this).next().empty();
        $(this).next().append(Get_Number(1, 12, false));
        $(this).next().next().empty();
        $(this).next().next().append(Get_Number(1, Get_Days(y, m), false));
    });
    $(".default[name=month][data-count='" + Num + "']").change(function () {
        y = parseInt($(this).prev().val(), 10);
        m = parseInt($(this).val(), 10);
        $(this).next().empty();
        $(this).next().append(Get_Number(1, Get_Days(y, m), false));
    });
}