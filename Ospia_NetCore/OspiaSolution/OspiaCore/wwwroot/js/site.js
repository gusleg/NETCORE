// Write your Javascript code.

//agrego para que las ventanas de edit,delete etc etc sean modal
$('#myModal').on('shown.bs.modal', function () {
    $('#myInput').focus()
})