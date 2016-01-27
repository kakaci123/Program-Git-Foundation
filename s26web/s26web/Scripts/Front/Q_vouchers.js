function Check() {
    var q2 = $("input[name=Q2]:checked").val()
    var q3 = $("input[name=Q3]:checked").val()
    var q7 = $("input[name=Q7]:checked").val()
    var q8 = $("input[name=Q8]:checked").val()
    var q2_note = $('input[name="Q2_Note"]').val()
    var q3_note = $('input[name="Q3_Note"]').val()
    var q7_note = $('input[name="Q7_Note"]').val()
    var q8_note = $('input[name="Q8_Note"]').val()
    if (q2 == "網站") {
        if (q2_note == null || q2_note == "") {
            $(".Check_Q2").css("display", "block");
            $('html,body').animate({ scrollTop: '400px' }, 0);
            return false;
        }
    }
    if (q3 == "有。品牌") {
        if (q3_note == null || q3_note == "") {
            $(".Check_Q3").css("display", "block");
            $('html,body').animate({ scrollTop: '600px' }, 0);
            return false;
        }
    }
    if (q7 == "不會。原因") {
        if (q7_note == null || q7_note == "") {
            $(".Check_Q7").css("display", "block");
            return false;
        }
    }
    if (q8 == "其他") {
        if (q8_note == null || q8_note == "") {
            $(".Check_Q8").css("display", "block");
            return false;
        }
    }

}
function Hide_Q2() {
    $(".Check_Q2").css("display", "None");
}
function Hide_Q3() {
        $(".Check_Q3").css("display", "None");
}
function Hide_Q7() {
    $(".Check_Q7").css("display", "None");
}
function Hide_Q8() {
    $(".Check_Q8").css("display", "None");
}