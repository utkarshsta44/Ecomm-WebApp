﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/GetAll',
            dataSrc: 'data'
        },
        "columns": [
            
            
            { data: 'productName', "width": "15%" },
            { data: 'productDescription', "width": "15%" },
            { data: 'productPrice', "width": "15%" },
            // Add this line for productImage
            { data: 'isAvailable', "width": "15%" }, // Add this line for isAvailable
            /*{ data: 'isActive', "width": "15%" },*/      // Add this line for isActive

            { data: 'isTrending', "width": "15%" },
            { data: 'categoryName', "width": "15%" },
            {
                data: 'productCreatedAt', "width": "15%", render: function (data, type, row) {
                    // Format the date using Moment.js
                    return moment(data).format('DD-MM-YYYY HH:mm:ss');
                }
            },
            {
                data: 'productId',
                render: function (data, type, row) {
                    var productId = row.productId; // Assuming you have a 'productId' property in your row data
                    var isAvailable = row.isAvailable; // Assuming you have an 'isAvailable' property in your row data

                    var updateBtn = `<a href="/Product/Update/${productId}" class="btn btn-warning mx-2">
                            <i class="bi bi-pencil-square"></i> Edit
                        </a>`;

                    var deactivateBtn = `<a href="/Product/Deactive/${productId}" class="btn btn-danger">
                                <i class="bi bi-trash-fill"></i> Deactive
                             </a>`;

                    var activateBtn = `<a href="/Product/Active/${productId}" class="btn btn-success">
                                <i class="bi bi-check-circle-fill"></i> Active
                           </a>`;

                    return `<div class="btn-group" role="group">
                    ${isAvailable ? updateBtn + deactivateBtn : updateBtn + activateBtn}
                </div>`;
                },
                width: "45%"
            }


        ],
        "order": [[4, 'desc']] // sort by the 'productCreatedAt' column in descending order
    });


    $('#IsAvailableCheckbox').on('change', function () {
        var isAvailableChecked = $(this).prop('checked');

        console.log('Checkbox state:', isAvailableChecked);

        // Use DataTable's search API to filter rows based on checkbox status
        var columnIndex = 3; // Change this to the correct index
        var searchTerm = isAvailableChecked ? 'true' : '';

        console.log('Applying search:', searchTerm);

        dataTable.column(columnIndex).search(searchTerm).draw();
    });
    //$('#IsTrendingCheckbox').on('change', function () {
    //    var isAvailableChecked = $(this).prop('checked');

    //    console.log('Checkbox state:', isTrendingChecked);

    //    // Use DataTable's search API to filter rows based on checkbox status
    //    var columnIndex = 4; // Change this to the correct index
    //    var searchTerm = isTrendingChecked ? 'true' : '';

    //    console.log('Applying search:', searchTerm);

    //    dataTable.column(columnIndex).search(searchTerm).draw();
    //});


    $('#FilterByCategory').on('change', function () {
        console.log('Change event triggered');
        var selectedCategory = $(this).val();

        // Clear both global and category search
        dataTable.search('').columns().search('').draw();

        // Apply category filter if not "All"
        if (selectedCategory !== 'all') {
            console.log('Applying category filter:', selectedCategory);
            dataTable.column(5).search(selectedCategory).draw(); // Assuming categoryName is at index 4
        }

        $.ajax({
            url: '/Home/Privacy',
            type: 'POST',
            data: { myData: selectedCategory },
            success: function (data) {
                sessionStorage.setItem('TheCategory', selectedCategory)
                console.log(data.success)
            },
            error: function () {
                alert("error");
            }
        });
    });


}