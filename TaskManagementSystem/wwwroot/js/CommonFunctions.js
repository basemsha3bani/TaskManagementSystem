function handleDelete(url) {
    if (confirm("Are you sure you want to delete?"))
    {
        window.location.href = url;
    }
}

    $(document).ready(function(){
        $('.dropdown-toggle').dropdown()
    });
