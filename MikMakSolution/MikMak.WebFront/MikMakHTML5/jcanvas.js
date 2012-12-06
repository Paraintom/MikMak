/*! jcanvas v1.0.0 | Tom from Oliv&Tom Corp. */

var borderPixel = 3;
var pixelBySquare = 150;
var canvas;

function drawCanvas(nbLin, nbCol)
{
	if(document.body.getElementsByTagName("canvas").length == 0){	
		canvas = document.createElement("canvas");
		canvas.style.position = "relative";
		canvas.style.left = "0px";
		canvas.style.top = "0px";
		canvas.style.border = "black "+borderPixel+"px solid";
	}
	var ctx = canvas.getContext("2d");

	var height = nbLin;
	var width = nbCol;
	
	canvas.width = width * pixelBySquare;
	canvas.height = height * pixelBySquare;
	
	//On trace les lignes
	for (i=1; i< height; i++)
	{
		var tab = new Array();
		tab[0] = 0;
		tab[1] = i*pixelBySquare;
		tab[2] = width*pixelBySquare;
		tab[3] = i*pixelBySquare;
		drawLine(ctx,tab);
	}
	
	//On trace les colonnes
	for (i=1; i< width; i++)
	{
		var tab = new Array();
		tab[0] = i*pixelBySquare;
		tab[1] = 0;
		tab[2] = i*pixelBySquare;
		tab[3] = height*pixelBySquare;
		drawLine(ctx,tab);
	}		
	canvas.id = "canvasGrid";
	canvas.addEventListener("click", gridOnClick, false);	
	setSpan('mainArea', canvas);
}  

function gridOnClick(e)
{
	var cell = getCursorPosition(e);
	var x =cell.column+1;
	var y = cell.row+1;
	query = "Play?sessionId="+sessionId+"&x="+x+"&y="+y;
	setOutput(x+' '+y)
	queryService(query,onGridReceived);	
}

function getCursorPosition(e) {

    var x;
    var y;
    if (e.pageX != undefined && e.pageY != undefined) {
		x = e.pageX;
		y = e.pageY;
    }
    else {
		x = e.clientX + document.body.scrollLeft +
				document.documentElement.scrollLeft;
		y = e.clientY + document.body.scrollTop +
				document.documentElement.scrollTop;
    }
	x -= canvas.offsetLeft;
    y -= canvas.offsetTop;
	var cell = new Cell(Math.floor(y/pixelBySquare),
                        Math.floor(x/pixelBySquare));
    return cell;
}

function Cell(row, column) {
    this.row = row;
    this.column = column;
}

function drawLine(context,tab)
{
	 context.beginPath();
	 context.moveTo(tab[0],tab[1]);
	 context.lineTo(tab[2],tab[3]);
	 context.stroke(); 
}  

function drawCircle(name,x,y)
{
	var context = canvas.getContext("2d");
	console.log("drawCircle "+name+" x "+x+" y "+y+" pixelBySquare"+pixelBySquare);
	context.beginPath(); // Le cercle extérieur
	context.lineWidth="20";
	context.fillStyle="#FF4422"
	context.arc(0*borderPixel+x*pixelBySquare+0.5*pixelBySquare,0*borderPixel+ y*pixelBySquare+0.5*pixelBySquare, 0.5*pixelBySquare, 0, Math.PI * 2); // Ici le calcul est simplifié
	//context.stroke();
	context.fill();
}

function drawLetter(name,x,y)
{
	x=x-1;
	y=y-1;
	var context = canvas.getContext("2d");
	//console.log("drawletter "+name+" x "+x+" y "+y+" pixelBySquare"+pixelBySquare);	
    context.font = "20pt Calibri,Geneva,Arial";	
    context.strokeText(name, 0*borderPixel+x*pixelBySquare+0.5*pixelBySquare, 0*borderPixel+ y*pixelBySquare+0.5*pixelBySquare);
}