const mapLoc = { lat: 32.0749951, lng: 34.7747892 };
let map;

function getMap() {
    map = new Microsoft.Maps.Map(document.getElementById("map"), {
        center: new Microsoft.Maps.Location(mapLoc.lat, mapLoc.lng),
        zoom: 10
    });

    initStores();
}

function initStores() {

    $.ajax({
        url: `/Home/GetStores`,
        type: 'GET',
    }).done(data => showStores(data));

}

function showStores(stores) {
    addStoresToMap(stores);

    // Getting the weather for the stor
    const storesList = stores.map(async (store) => {
        const temp = await $.ajax({
            url: `/Home/GetTemprature?lon=${store.lontitude}&lat=${store.latitude}`,
            type: 'GET'
        });

        let template = $('#store-template').html();
        var currStore = template;
        console.log(store);

        $.each(store, (key, value) => {
            currStore.replaceAll(`{${key}}`, value);
        });
        currStore.replaceAll("{temp}", temp);

        return currStore;
    });

    Promise.all(storesList).then(rows => {
        const storList = $('#storesList');
        storList.html(rows.join(''));
    })
}

function addStoresToMap(stores) {
    stores.forEach(store => {
        // Setting the pushpin
        const loc = new Microsoft.Maps.Location(store.latitude, store.lontitude);
        const pin = new Microsoft.Maps.Pushpin(loc);
        map.entities.push(pin);
    })
}