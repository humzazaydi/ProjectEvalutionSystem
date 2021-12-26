$('#ddlStudents').select2({
    allowClear: true
});
$('#ddlTeachers').select2({
    allowClear: true
});

$(document).ready(function () {
    AjaxCall('/Assignments/GetDropdownDataset', null, 'GET', onSuccess);
    function onSuccess(data) {
        console.log(data);
    }
})