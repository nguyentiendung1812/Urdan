///<reference path="../../lib/jquery/dist/jquery.min.js" />


// Delete user
function handleDeleteUser(id) {
    const isConfirm = confirm("Delete this user?")
    if (isConfirm) {
        $.ajax({
            type: "GET",
            url: `/Admin/HandleDeleteUser/${id}`,
            contentType: "application/json",
            success: function () {
                $(`#${id}`).closest("tr").remove();
            },
            error: function () {
                alert("Error while deleting data: " + error.responseText);
            }
        })
    }
}

// Delete product
function handleDeleteProduct(id) {
    const isConfirm = confirm("Delete this product?");
    if (isConfirm) {
        $.ajax({
            type: "GET",
            url: `/Admin/HandleDeleteProduct/${id}`,
            contentType: "application/json",
            success: function () {
                $(`#${id}`).closest("tr").remove();
            },
            error: function (error) {
                console.log(error);
                alert("Error while deleting data: " + error.responseText)
            }
        })
    }
}

// Delete image
function handleDeleteImage(id) {
    const isConfirm = confirm("Delete this image?")
    if (isConfirm) {
        $.ajax({
            type: "GET",
            url: `/Admin/HandleDeleteImage/${id}`,
            contentType: "application/json",
            success: function () {
                $(`#${id}`).closest("tr").remove();
            },
            error: function (error) {
                console.log(error);
                alert("Error while deleting data: " + error.responseText);
            }
        })
    }
}


// Delete address
function handleDeleteAddress(id) {
    const isConfirm = confirm("Delete this address?");
    if (isConfirm) {
        $.ajax({
            type: "GET",
            url: `/Admin/HandleDeleteAddress/${id}`,
            contentType: "application/json",
            success: function () {
                $(`#${id}`).closest("tr").remove();
            },
            error: function (error) {
                console.log(error);
                alert("Error while deleting data: " + error.responseText)
            }
        })
    }
}

// Delete color 
function handleDeleteColor(id) {
    const isConfirm = confirm("Delete this color?");
    if (isConfirm) {
        $.ajax({
            type: "GET",
            url: `/Admin/HandleDeleteColor/${id}`,
            contentType: "application/json",
            success: function () {
                $(`#${id}`).closest("tr").remove();
            },
            error: function (error) {
                console.log(error);
                alert("Error while deleting data: " + error.responseText)
            }
        })
    }
}


// Delete brand
function handleDeleteBrand(id) {
    const isConfirm = confirm("Delete this brand?");
    if (isConfirm) {
        $.ajax({
            type: "GET",
            url: `/Admin/HandleDeleteBrand/${id}`,
            contentType: "application/json",
            success: function () {
                $(`#${id}`).closest("tr").remove();
            },
            error: function (error) {
                console.log(error);
                alert("Error while deleting data: " + error.responseText)
            }
        })
    }
}









