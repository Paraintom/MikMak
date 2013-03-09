var loginCookieName="MikMak.login";
var pwdCookieName="MikMak.pwd";

function createCookie(name,value,days) {
	if (days) {
		var date = new Date();
		date.setTime(date.getTime()+(days*24*60*60*1000));
		var expires = "; expires="+date.toGMTString();
	}
	else var expires = "";
	document.cookie = name+"="+value+expires+"; path=/";
}

function readCookie(name) {
	var nameEQ = name + "=";
	var ca = document.cookie.split(';');
	for(var i=0;i < ca.length;i++) {
		var c = ca[i];
		while (c.charAt(0)==' ') c = c.substring(1,c.length);
		if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
	}
	return null;
}

function eraseCookie(name) {
	createCookie(name,"",-1);
}

function credentialAlreadySaved(){
	return (readCookie(loginCookieName) != null && readCookie(pwdCookieName) != null);	
}

function resetCookies(){
	eraseCookie(loginCookieName);
	eraseCookie(pwdCookieName);
}

function setLoginCookie(value){
	createCookie(loginCookieName,value,30);
}
function setPasswordCookie(value){
	createCookie(pwdCookieName,value,30);
}

function getLoginCookie(){
	return readCookie(loginCookieName);
}
function getPasswordCookie(){
	return readCookie(pwdCookieName);
}
