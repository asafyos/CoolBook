const NAME = "Name";
const CATEGORY = "Categories";
const AUTHOR = "AuthorId";
const PARAMETERS = [CATEGORY, AUTHOR, NAME];

$(() => {
    intializePage();
    getData();
    //TODO show & hide loader
})

const onChangeInput = (id) => () => {
    const value = $(`#${id}`).val();
    reloadBooks();
}

const initializeListeners = () => {
    PARAMETERS.forEach(p => {
        switch (p) {
            case NAME:
                $(`#${p}`).keyup(onChangeInput(p));
                break;
            case CATEGORY:
            case AUTHOR:
                $(`#${p}`).change(onChangeInput(p));
                break;
            default:
                break;
        }
    })
}

const intializePage = () => {
    PARAMETERS.forEach(f => {
        let param;
        if (param = getUrlParameter(f)) {
            switch (f) {
                case NAME:
                    $(`#${f}`).val(param);
                    break;
                case CATEGORY:
                case AUTHOR:
                    $(`#${f} option[value='${param}']`).prop("selected", true);
                    break;
                default:
                    break;
            }
        }
    });
}

let delayTimer;

const reloadBooks = () => {
    clearTimeout(delayTimer);
    delayTimer = setTimeout(getData, 200);
}

const getData = () => {
    $.ajax({
        url: "/Books/Find",
        data: PARAMETERS.reduce((obj, f) => {
            let param = prepareParam($(`#${f}`).val());
            if (!param) return obj;

            obj[f] = prepareParam($(`#${f}`).val());
            return obj;
        }, {})
    }).done(setBooks)
}

const setBooks = (data) => {
    const element = $('#cb-search-container');

    const template = $('#cb-card-template').html();

    let newHtml = '';

    if (data.length > 0) {
        data.forEach(book => {
            let temp = template;

            temp = temp
                .replaceAll('{link}', `/Books/Details/${book.id}`)
                .replaceAll('{image}', book.imageUrl)
                .replaceAll('{price}', `&#8362;${book.price.toFixed(2)}`)
                .replaceAll('{title}', book.name)
                .replaceAll('{text1}', `<i>${book.author.name}</i>`)
                .replaceAll('{text2}', book.categories.map(c => c.name).join(', '));

            newHtml += temp;
        });
    } else {
        newHtml = '<h2 class="cb-no-books">No Results</h2>';
    }

    $(".cb-book-card").remove();
    $(".cb-no-books").remove();
    element.append(newHtml);

    initializeListeners();
}

const prepareParam = p => {
    if (Array.isArray(p)) {
        return p.join(',');
    }

    return p;
}

const getUrlParameter = (sParam) => {
    let sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return typeof sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }

    return false;
};