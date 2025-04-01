let graphElement;

window.setup = (id, config) => {

    //let ctx = document.getElementById(id).getContext('2d');
    let ctx = document.getElementById(id);
    graphElement = new Chart(ctx, config);
    console.log("Rendering a chart.");
}

window.reset = () => {
    if (graphElement instanceof Chart) {
        graphElement.destroy();
    }
}

window.dispose = () => {
    if (graphElement instanceof Chart) {
        graphElement.destroy();
    }
}