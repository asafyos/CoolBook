$("#cb-review-form").submit(e => {
    if ($("input[name=Rate]").val() == 0) {
        $("#review-rating").removeAttr("hidden");
        e.preventDefault();
    }
})