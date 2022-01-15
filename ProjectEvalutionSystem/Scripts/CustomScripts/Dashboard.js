var datatable;
$(document).ready(function () {
    GetStats();
    GetAllAssignmentDetails();
})

function GetStats() {

    AjaxCall('/Home/GetStats', null, "GET", onSuccess)
    function onSuccess(data) {
         
        $('#totalstudents').prop('Counter', 0).animate({
            Counter: data.students
        }, {
            duration: 4000,
            easing: 'swing',
            step: function (now) {
                $('#totalstudents').text(Math.ceil(now).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
            }
        });

        $('#totalteachers').prop('Counter', 0).animate({
            Counter: data.teachers
        }, {
            duration: 4000,
            easing: 'swing',
            step: function (now) {
                $('#totalteachers').text(Math.ceil(now).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
            }
        });

        $('#totalassignments').prop('Counter', 0).animate({
            Counter: data.assignments
        }, {
            duration: 4000,
            easing: 'swing',
            step: function (now) {
                $('#totalassignments').text(Math.ceil(now).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
            }
        });

        $('#totalevaluation').prop('Counter', 0).animate({
            Counter: data.evalutionIndexes
        }, {
            duration: 4000,
            easing: 'swing',
            step: function (now) {
                $('#totalevaluation').text(Math.ceil(now).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
            }
        });
        $('#totalcourses').prop('Counter', 0).animate({
            Counter: data.courses
        }, {
            duration: 4000,
            easing: 'swing',
            step: function (now) {
                $('#totalcourses').text(Math.ceil(now).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
            }
        });
    }
}


function GetAllAssignmentDetails() {
    AjaxCall("/Home/GetAllAssignmentDetails", null, "GET", onSuccess);
    function onSuccess(data) {
         
        datatable = $('#dashboardAssignmentTable').KTDatatable({
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
                field: 'Name',
                title: 'Assignment Name'
            }, {
                field: 'Description',
                title: 'Description'
            }, {
                field: 'IsDeleted',
                title: 'Status',
            }, {
                field: 'CreationTimeStamp',
                title: 'Created Date',
                type: 'date',
                format: 'MM/DD/YYYY',
            }],
        });
    }
}