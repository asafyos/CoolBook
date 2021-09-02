const SEARCH_PARAMS = ["book", "rate", "review"];

const getSetParam = p => {
    const value = getUrlParameter(p);

    if (value) {
        $(`input[name=${p}]`).val(value);
    }
}

$(() => {
    SEARCH_PARAMS.forEach(getSetParam);
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

    return false;
};