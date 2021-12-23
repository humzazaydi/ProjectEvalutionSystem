var EvalutionIndexTable;
$('#ddlassignments').select2({
    allowClear: true
});
$('#ddlstudents').select2({
    allowClear: true
});
$('#ddlteachers').select2({
    allowClear: true
});

var KTDatatableDataLocalDemo = function () {
    // Private functions

    // demo initializer
    var demo = function () {
        AjaxCall("/EvalutionIndex/GetAll", null, "GET", onSuccess);
        function onSuccess(data) {
            EvalutionIndexTable = $('#kt_datatable').KTDatatable({
                // datasource definition
                data: {
                    type: 'local',
                    source: data,
                    pageSize: 10,
                },

                // layout definition
                layout: {
                    scroll: false, // enable/disable datatable scroll both horizontal and vertical when needed.
                    // height: 450, // datatable's body's fixed height
                    footer: false, // display/hide footer
                },

                // column sorting
                sortable: true,

                pagination: true,

                // columns definition
                columns: [{
                    field: 'ID',
                    title: '#',
                    sortable: false,
                    width: 20,
                    type: 'number',
                    selector: true,
                    textAlign: 'center',
                }, {
                    field: '',
                    title: 'Submission Date',
                    template: function (data) {
                        return moment(data.SubmissionDate).format('YYYY-MM-DD');
                    }
                }, {
                    field: '',
                    title: 'Evalution Date',
                    template: function (data) {
                        return moment(data.EvalutionDate).format('YYYY-MM-DD');
                    }
                }, {
                    field: 'Remarks',
                    title: 'Remarks',
                }, {
                    field: 'Comments',
                    title: 'Comments',
                }, {
                    field: 'StudentName',
                    title: 'Student',
                }, {
                    field: 'TeacherName',
                    title: 'Teacher',
                }, {
                    field: 'AssignmentName',
                    title: 'Assignment',
                }
                ],
            });
        }


        $('#kt_datatable_search_status').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'Status');
        });

        $('#kt_datatable_search_type').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'Type');
        });

        $('#kt_datatable_search_status, #kt_datatable_search_type').selectpicker();
    };

    return {
        // Public functions
        init: function () {
            // init dmeo
            demo();
        },
    };
}();

jQuery(document).ready(function () {
    KTDatatableDataLocalDemo.init();
    GetDropdownDataStudent();
});

$('#kt_datatable').on('click', 'tr td', function (n) {
    if (EvalutionIndexTable.row(this).data() !== 'undefined') {
        console.log(EvalutionIndexTable.row(this).data());
        //location.href = '/Consumers/ConsumerDetails/' + parseInt(consumerTable.row(this).data().belongToId);
    }
});

function GetDropdownDataStudent() {
    AjaxCall("/EvalutionIndex/GetDropdownDataStudent", null, "GET", onSuccess);
    function onSuccess(data) {
        $.each(data, function () {
            $('#ddlstudents').append($("<option     />").val(this.ID).text(this.FullName));
        })
            $('#ddlstudents').val(null).trigger('change');
    }
}
$('#ddlstudents').on("select2:select", function (event) {
    GetDropdownDataForStudent($('#ddlstudents :selected').val());
})
$('#ddlteachers').click(function () {
    GetDropdownDataForStudent($('#ddlstudents :selected').val());
})

$('#ddlassignments').on("select2:select", function (event) {
    $('#txtAssignmentName').text($('#ddlassignments :selected').text() == "" ? "N/A" : $('#ddlassignments :selected').text());
})
$('#ddlassignments').on("select2:unselect", function (event) {
    $('#txtAssignmentName').text($('#ddlassignments :selected').text() == "" ? "N/A" : $('#ddlassignments :selected').text());
})

$('#ddlteachers').on("select2:select", function (event) {
    $('#txtTeacherName').text($('#ddlteachers :selected').text() == "" ? "N/A" : $('#ddlteachers :selected').text());
})
$('#ddlteachers').on("select2:unselect", function (event) {
    $('#txtTeacherName').text($('#ddlteachers :selected').text() == "" ? "N/A" : $('#ddlteachers :selected').text());
})

$('#ddlstudents').on("select2:select", function (event) {
    GetDropdownDataForStudent($('#ddlstudents :selected').val());
})
function GetDropdownDataForStudent(id) {
    AjaxCall("/EvalutionIndex/GetDropdownDataForStudent?studentID=" + parseInt(id), null, "GET", onSuccess);
    function onSuccess(data) {
        $.each(data.teachers, function () {
            $('#ddlteachers').append($("<option     />").val(this.ID).text(this.FullName));
        })
        $('#ddlteachers').val(null).trigger('change');
        $.each(data.assignments, function () {

            $('#ddlassignments').append($("<option     />").val(this.ID).text(this.Name));
        })
        $('#ddlassignments').val(null).trigger('change');
    }
}

$('#btnStartEva').click(function () {
    if ($('#ddlstudents :selected').text() == "") {
        toastr.error("Please Select Student");
    }
    else if ($('#ddlteachers :selected').text() == "") {
        toastr.error("Please Select Teacher");
    }
    else if ($('#ddlassignments :selected').text() == "") {
        toastr.error("Please Select Assignment");
    }
    else {
        let url = `/EvalutionIndex/StartEvalution?studentid=${$('#ddlstudents :selected').val()}
                &teacherid=${$('#ddlteachers :selected').val()}&assignmentid=${$('#ddlassignments :selected').val()}`;
        AjaxCall(url
            , null, 'GET',
            onSuccess
        )
        function onSuccess(data) {
            debugger;
        }
    }
});