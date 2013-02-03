function paintProgress(id, percent, legend) {
    var canvas = document.getElementById('c' + id);
    var context = false;
    if (canvas.getContext && (context = canvas.getContext('2d'))) {
        document.getElementById('p' + id).style.display = "none";
        document.getElementById('legend' + id).style.textAlign = "center";
        canvas.style.display = "block";

        var w = canvas.width = canvas.clientWidth;
        var h = canvas.height = canvas.clientHeight;
        var size = (w > h) ? h : w;

        var centerX = canvas.width / 2;
        var centerY = canvas.height / 2;
        var radius = size / 3;

        context.font = '1em sans-serif';
        context.textAlign = 'center';
        context.textBaseline = 'middle';
        context.fillText(legend, centerX, centerY);

        context.beginPath();
        context.arc(centerX, centerY, radius, 0, 2 * Math.PI, false);
        context.lineWidth = 10;
        context.strokeStyle = '#ddd';
        context.stroke();

        context.beginPath();
        context.arc(centerX, centerY, radius, 0, (2 * Math.PI) * percent, false);
        context.lineWidth = 10;
        context.strokeStyle = '#0dd';
        context.stroke();
    }
    else {
        var canvas = document.getElementById('p' + id);
        canvas.style.display = "block";
        var legend = document.getElementById('legend' + id);
        legend.style.textAlign = "left";
    }
}
