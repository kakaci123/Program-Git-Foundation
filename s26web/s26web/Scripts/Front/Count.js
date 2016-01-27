function Check() {
    if ($("#Code").val() == null || $("#Code").val() == "") {
        $(".Check").css("display", "block");
        return false;
    }

}
function Hide() {
    if ($("#Code").val() != null) {
        $(".Check").css("display", "None");
        return false;
    }
}