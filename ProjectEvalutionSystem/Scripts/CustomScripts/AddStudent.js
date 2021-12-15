$(document).ready(function () {
    GetDropdownDataStudent();
});
function GetDropdownDataStudent() {
    AjaxCall("/Teachers/GetAll", null, "GET", onSuccess);
        function onSuccess(data) {
            $.each(data, function () {
                $('#ddlTeacher').append(`<option name="TeacherID" value="${this.ID}">${this.FullName}</option>`);
            })
        }
    }