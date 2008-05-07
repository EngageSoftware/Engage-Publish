/*   Browser Detection   */

var agt = navigator.userAgent.toLowerCase();
var w3c = (document.getElementById) ? true : false;
var xxx = ((agt.indexOf('opera') != -1) || (agt.indexOf('webtv') != -1) || (agt.indexOf('omniweb') != -1)) ? true : false;

var ieX = ((agt.indexOf('msie')  != -1) && w3c && !xxx) ? true : false;
var ie4 = ((agt.indexOf('msie')  != -1) && (document.all) && !w3c && !xxx) ? true : false;
var ns6 = ((agt.indexOf('gecko') != -1) && w3c && !xxx) ? true : false;
var ns4 = (document.layers && !w3c && !xxx) ? true : false;


/*   div overs   */
function getRef (obj) {
	if(typeof obj == "string")
		obj = (w3c) ? document.getElementById(obj) : document.all(obj);
	return obj;
}

function cnSwap (elm, nc) {
	getRef(elm).className = nc;
}

//image functions
var preloadFlag = false;
function newImage(arg) {
	if (document.images) {
		rslt = new Image();
		rslt.src = arg;
		return rslt;
	}
}

// rollover functions
function findElement(n,ly) {
	if (browserVers < 4)		return document[n];
	var curDoc = ly ? ly.document : document;
	var elem = curDoc[n];
	if (!elem) {
		for (var i=0;i<curDoc.layers.length;i++) {
			elem = findElement(n,curDoc.layers[i]);
			if (elem) return elem;
		}
	}
	return elem;
}

function changeImages() {
	if (document.images && (preloadFlag == true)) {
		var img;
		for (var i=0; i<changeImages.arguments.length; i+=2) {
			img = null;
			if (document.layers) {
				img = findElement(changeImages.arguments[i],0);
			}
			else {
				img = document.images[changeImages.arguments[i]];
			}
			if (img) {
				img.src = changeImages.arguments[i+1];
			}
		}
	}
}
