$(() => {
    if (search) {
        const search = getUrlParameter("search");
        const input = $("input#search");
        input.val(search).focus()
    }
})

const getUrlParameter = (sParam) => {
    let sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return typeof sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]).split("+").join(" ");
        }
    }

    return "";
};