
var pixelByLine = 40;
var borderPixel = 3;
var maxClick;
var allClick;
var rootpath="http://localhost:33226/api/";

// ************  SessionId Managment ****************
var sessionIdNotSet = 'notSet';
var sessionId = sessionIdNotSet;

var getSessionId=function()
{
	return sessionId;
}

var setSessionId=function(data)
{	
	sessionId = data;
	if(sessionId == sessionIdNotSet){
		setOutput('Disconnected');
	}
	else
	{
		setFooter('You are connected, sessionId = '+sessionId);	
	}
}

// ************  Connection ****************

var connect=function(login,mdp)
{
	query = "Connect?login="+login+"&password="+mdp;
	queryService(query,onConnect);
}

var onConnect=function(data)
{	
	if((data.split("-").length ) == 5){
		hideById('formConnect');
		setSessionId(data);
		query = "GetListBattles?sessionId="+data;
		queryService(query,onListGameReceived);
	}
	else{
		setSessionId('notSet');
		setOutput(data);
	}
}

// ************  Game list retrieving ****************
var onListGameReceived=function(data)
{
	if(typeof data.ExceptionMessage != 'undefined'){	
		setOutput(data.ExceptionMessage);
	}
	else
	{	
		var listBattleSpan = document.createElement('span');
		listBattleSpan.setAttribute('id', 'listBattle');		
		for (var i = 0; i < data.length; i++) {
			element = data[i];
			//writeObj(element);
			addToListGame(element,listBattleSpan);
		}
		setSpan('mainArea', listBattleSpan);
		
		//Create Game
		
		var inputLogin = document.createElement('input');		
		inputLogin.setAttribute('placeholder', 'opponent login');
		inputLogin.setAttribute('id', 'opponentLogin');
		inputLogin.setAttribute('type', 'text');
		addSpan('mainArea', inputLogin);
		
		/*var buttonCreateGame = document.createElement('input');		
		buttonCreateGame.setAttribute('type', 'button');
		buttonCreateGame.setAttribute('onclick', 'createGame()');
		buttonCreateGame.value = 'Create new game';*/
		var buttonCreateGame = createButton('createGame(1)', 'Create new Tic tac toe game');
		addSpan('mainArea', buttonCreateGame);
		var buttonCreateGame2 = createButton('createGame(2)', 'Create new Chess game');
		addSpan('mainArea', buttonCreateGame2);
		var buttonCreateGame3 = createButton('createGame(3)', 'Create new Connect Four game');
		addSpan('mainArea', buttonCreateGame3);
		
		setOutput('number of Battle received : '+data.length);
	}
}

var createButton=function(action, text){
	var buttonCreateGame = document.createElement('input');		
	buttonCreateGame.setAttribute('type', 'button');
	buttonCreateGame.setAttribute('onclick', action);
	buttonCreateGame.value = text;
	return buttonCreateGame;
}

var createGame=function(type)
{
	var opponentLogin = document.getElementById('opponentLogin');	
	setOutput('Creating game...');
	query = "CreateBattle?sessionId="+sessionId+"&typeGame="+type+"&opponent1="+opponentLogin.value;
	queryService(query,onGridReceived);
}

var addToListGame=function(data,listBattleSpan)
{
    var newSpan = document.createElement('span');
    // add the class to the 'span'
    newSpan.setAttribute('class', 'gameItem');
    newSpan.setAttribute('onclick', 'onGameSelected("'+data.Battle.GameId+'")');
	newSpan.innerHTML = 'GameId ='+data.Battle.GameId+', Game type ='+data.Battle.GameTypeString+', CreationTime='+new Date(data.Battle.CreationTime).toString("yyyy-MM-dd HH:mm:ss")+', Last Update='+new Date(data.Battle.LastUpdate).toString("yyyy-MM-dd HH:mm:ss")+', opponents :';
	
	for (var i = 0; i < data.InGame.length; i++) {	
		newSpan.innerHTML += data.InGame[i].Login+',';
	}
	newSpan.innerHTML += '<br>';
    listBattleSpan.appendChild(newSpan);	
}

// *****************  Game selection ****************
var onGameSelected=function(gameId)
{
	setOutput('Battle selected '+gameId+', connecting to game...');
	query = "GetGrid?sessionId="+sessionId+"&gameId="+gameId;
	queryService(query,onGridReceived);
}
var refreshCurrentGame=function()
{
	setOutput('Session Id selected '+sessionId+', connecting to game...');
	query = "GetGrid?sessionId="+sessionId;
	queryService(query,onGridReceived);
}

// ************  Grid Received ****************

var numLines;
var onGridReceived=function(data)
{
	if(typeof data.ExceptionMessage != 'undefined'){	
		setOutput(data.ExceptionMessage);
	}
	else
	{	
		//writeObj(data);		
		setSessionId(data.SessionId);
		var grid = data.State;
		maxClick = grid.MoveNumber;
		allClick = new Array();
		numLines = grid.NumberLines;
		var numCol = grid.NumberColumns;
		//Draw Canvas
		drawCanvas(numLines,numCol);
		//Draw Pawns
		var pawns = data.State.PawnLocations;
		for (var i = 0; i < pawns.length; i++) {
			element = pawns[i];
			//writeObj(element);
			drawLetter(element.Name,element.Coord.x,element.Coord.y);
		}
		
		//addSpan('mainArea', '<br>');
		var buttonCreateGame = createButton('refreshCurrentGame()', 'Refresh');
		var buttonReturnToGameList = createButton('onConnect("'+data.SessionId+'")', 'Return To game list');
		addSpan('mainArea', buttonCreateGame);
		addSpan('mainArea', buttonReturnToGameList);
		
		setOutput(grid.CurrentMessage.Information);
	}
}

// ***********************  Helpers *********************** 
function writeObj(obj, message) {
  if (!message) { message = obj; }
  var details = "*****************" + "\n" + message + "\n";
  var fieldContents;
  for (var field in obj) {
    fieldContents = obj[field];
    if (typeof(fieldContents) == "function") {
      fieldContents = "(function)";
    }
    details += "  " + field + ": " + fieldContents + "\n";
  }
  console.log(details);
}

function hideById(id) {
	if(document.getElementById(id) != null){
		document.getElementById(id).style.display = 'none';
	}
}

function setOutput(text)
{
	setElementText("output",text);
}
function setFooter(text)
{
	setElementText("footer",text);
}
function setElementText(id, text)
{
	document.getElementById(id).innerHTML = text;
}

function setSpan(name, content)
{
	var span = document.getElementById(name);
	while( span.firstChild ) {
		span.removeChild( span.firstChild );
	}
	addSpan(name,content);
}

function addSpan(name, content)
{
	var span = document.getElementById(name);
	span.appendChild( content );
}

function queryService(query, callBack)
{
	$.ajax(
		{
			url: rootpath+query,
			dataType: 'jsonp',
			success: callBack,
			error:OnError
		}
	);
}
var OnError = function (event, jqXHR, ajaxSettings, thrownError) {
    alert('[event:' + event + '], [jqXHR:' + jqXHR + '], [ajaxSettings:' + ajaxSettings + '], [thrownError:' + thrownError + '])');
}

/*! jcanvas v1.0.0 | Tom from Oliv&Tom Corp. */

var borderPixel = 3;
var pixelBySquare = 80;
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
	var x = cell.column + 1;
	var y = numLines - cell.row;
	var tab = new Array();
	tab.push(x);
	tab.push(y);
	allClick.push(tab);
	
	if(allClick.length != maxClick){
		setOutput('Click recorded : '+allClick.length+' on '+maxClick);
	}
	else
	{
		query = "Play?sessionId="+sessionId;
		for(var i=0;i<allClick.length;i++)
		{			
			query+="&x"+(i+1)+"="+allClick[i][0]+"&y"+(i+1)+"="+allClick[i][1];
		}
		queryService(query,onGridReceived);
		allClick = new Array();
	}
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
	y= numLines -y;
	var context = canvas.getContext("2d");
	//console.log("drawletter "+name+" x "+x+" y "+y+" pixelBySquare"+pixelBySquare);	
	if(name.charAt(0) == '1' || name.charAt(0) == '0'){
		// Scale the image to span the entire 500 x 500 canvas.
		var myImg = new Image();
		myImg.src = 'ChessGif/'+name+'.gif';
		

		myImg.onload = function() {
			context.drawImage(myImg, borderPixel/2 + x*pixelBySquare, borderPixel/2 + y*pixelBySquare, pixelBySquare - borderPixel, pixelBySquare - borderPixel);
		};
		//myImg.src = 'http://www.html5canvastutorials.com/demos/assets/darth-vader.jpg';
		//context.drawImage(myImg, 0*borderPixel+x*pixelBySquare+0.5*pixelBySquare,0*borderPixel+ y*pixelBySquare+0.5*pixelBySquare, 0.5*pixelBySquare, 0);		
        //context.drawImage(myImg, borderPixel + x*pixelBySquare, borderPixel + y*pixelBySquare, pixelBySquare - borderPixel, pixelBySquare - borderPixel);
	}
	else
	{	
		context.font = "20pt Calibri,Geneva,Arial";	
		context.strokeText(name, 0*borderPixel+x*pixelBySquare+0.5*pixelBySquare, 0*borderPixel+ y*pixelBySquare+0.5*pixelBySquare);
	}
}