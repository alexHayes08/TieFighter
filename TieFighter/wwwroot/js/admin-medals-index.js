$(function () {
    $(function () {
        $(document).ready(function () {
            $('#table').DataTable({
                columnDefs: [
                    {
                        orderable: false,
                        targets: 0
                    },
                    {
                        orderable: false,
                        targets: 1
                    },
                    {
                        orderable: false,
                        targets: 4
                    }
                ],
                select: {
                    style: "os",
                    selector: "td:first-child",
                    items: "col"
                },
                order: [[1, 'asc']]
            });
        });
    });

    $("#selectAll").on("change", function () {

        //console.log($(this).prop("checked"));

        if ($(this).prop("checked")) {

            // Select all medals
            $("#table tbody tr input[type='checkbox']").prop("checked", true);

        } else {

            // Deselect all medals
            $("#table tbody tr input[type='checkbox']").prop("checked", false);
        }
    });
});