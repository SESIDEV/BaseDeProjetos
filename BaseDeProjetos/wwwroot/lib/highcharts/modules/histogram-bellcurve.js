/*
 Highcharts JS v8.1.2 (2020-06-16)

 (c) 2010-2019 Highsoft AS
 Author: Sebastian Domas

 License: www.highcharts.com/license
*/
(function (a) { "object" === typeof module && module.exports ? (a["default"] = a, module.exports = a) : "function" === typeof define && define.amd ? define("highcharts/modules/histogram-bellcurve", ["highcharts"], function (c) { a(c); a.Highcharts = c; return a }) : a("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (a) {
    function c(f, a, d, c) { f.hasOwnProperty(a) || (f[a] = c.apply(null, d)) } a = a ? a._modules : {}; c(a, "mixins/derived-series.js", [a["parts/Globals.js"], a["parts/Utilities.js"]], function (a, c) {
        var f = c.addEvent, p = c.defined,
        l = a.Series; return {
            hasDerivedData: !0, init: function () { l.prototype.init.apply(this, arguments); this.initialised = !1; this.baseSeries = null; this.eventRemovers = []; this.addEvents() }, setDerivedData: a.noop, setBaseSeries: function () { var b = this.chart, a = this.options.baseSeries; this.baseSeries = p(a) && (b.series[a] || b.get(a)) || null }, addEvents: function () {
                var b = this; var a = f(this.chart, "afterLinkSeries", function () {
                    b.setBaseSeries(); b.baseSeries && !b.initialised && (b.setDerivedData(), b.addBaseSeriesEvents(), b.initialised =
                        !0)
                }); this.eventRemovers.push(a)
            }, addBaseSeriesEvents: function () { var a = this; var c = f(a.baseSeries, "updatedData", function () { a.setDerivedData() }); var d = f(a.baseSeries, "destroy", function () { a.baseSeries = null; a.initialised = !1 }); a.eventRemovers.push(c, d) }, destroy: function () { this.eventRemovers.forEach(function (a) { a() }); l.prototype.destroy.apply(this, arguments) }
        }
    }); c(a, "modules/histogram.src.js", [a["parts/Utilities.js"], a["mixins/derived-series.js"]], function (a, c) {
        function f(a) {
            return function (h) {
                for (var b =
                    1; a[b] <= h;)b++; return a[--b]
            }
        } var p = a.arrayMax, l = a.arrayMin, b = a.correctFloat, m = a.isNumber, k = a.merge, r = a.objectEach; a = a.seriesType; var g = { "square-root": function (a) { return Math.ceil(Math.sqrt(a.options.data.length)) }, sturges: function (a) { return Math.ceil(Math.log(a.options.data.length) * Math.LOG2E) }, rice: function (a) { return Math.ceil(2 * Math.pow(a.options.data.length, 1 / 3)) } }; a("histogram", "column", {
            binsNumber: "square-root", binWidth: void 0, pointPadding: 0, groupPadding: 0, grouping: !1, pointPlacement: "between",
            tooltip: { headerFormat: "", pointFormat: '<span style="font-size: 10px">{point.x} - {point.x2}</span><br/><span style="color:{point.color}">\u25cf</span> {series.name} <b>{point.y}</b><br/>' }
        }, k(c, {
            setDerivedData: function () { var a = this.baseSeries.yData; a.length && (a = this.derivedData(a, this.binsNumber(), this.options.binWidth), this.setData(a, !1)) }, derivedData: function (a, e, c) {
                var h = p(a), g = b(l(a)), n = [], d = {}, k = []; c = this.binWidth = b(m(c) ? c || 1 : (h - g) / e); this.options.pointRange = Math.max(c, 0); for (e = g; e < h && (this.userOptions.binWidth ||
                    b(h - e) >= c || 0 >= b(b(g + n.length * c) - e)); e = b(e + c))n.push(e), d[e] = 0; 0 !== d[g] && (n.push(b(g)), d[b(g)] = 0); var q = f(n.map(function (a) { return parseFloat(a) })); a.forEach(function (a) { a = b(q(a)); d[a]++ }); r(d, function (a, h) { k.push({ x: Number(h), y: a, x2: b(Number(h) + c) }) }); k.sort(function (a, h) { return a.x - h.x }); return k
            }, binsNumber: function () { var a = this.options.binsNumber, b = g[a] || "function" === typeof a && a; return Math.ceil(b && b(this.baseSeries) || (m(a) ? a : g["square-root"](this.baseSeries))) }
        })); ""
    }); c(a, "modules/bellcurve.src.js",
        [a["parts/Utilities.js"], a["mixins/derived-series.js"]], function (a, c) {
            function d(a) { var b = a.length; a = a.reduce(function (a, b) { return a + b }, 0); return 0 < b && a / b } function f(a, b) { var c = a.length; b = m(b) ? b : d(a); a = a.reduce(function (a, c) { c -= b; return a + c * c }, 0); return 1 < c && Math.sqrt(a / (c - 1)) } function l(a, b, c) { a -= b; return Math.exp(-(a * a) / (2 * c * c)) / (c * Math.sqrt(2 * Math.PI)) } var b = a.correctFloat, m = a.isNumber, k = a.merge; a = a.seriesType; a("bellcurve", "areaspline", { intervals: 3, pointsInInterval: 3, marker: { enabled: !1 } }, k(c,
                { setMean: function () { this.mean = b(d(this.baseSeries.yData)) }, setStandardDeviation: function () { this.standardDeviation = b(f(this.baseSeries.yData, this.mean)) }, setDerivedData: function () { 1 < this.baseSeries.yData.length && (this.setMean(), this.setStandardDeviation(), this.setData(this.derivedData(this.mean, this.standardDeviation), !1)) }, derivedData: function (a, b) { var c = this.options.intervals, e = this.options.pointsInInterval, d = a - c * b; c = c * e * 2 + 1; e = b / e; var f = [], g; for (g = 0; g < c; g++)f.push([d, l(d, a, b)]), d += e; return f } }));
            ""
        }); c(a, "masters/modules/histogram-bellcurve.src.js", [], function () { })
});
//# sourceMappingURL=histogram-bellcurve.js.map