/*
 Highcharts JS v8.1.2 (2020-06-16)

 Item series type for Highcharts

 (c) 2019 Torstein Honsi

 License: www.highcharts.com/license
*/
(function (b) { "object" === typeof module && module.exports ? (b["default"] = b, module.exports = b) : "function" === typeof define && define.amd ? define("highcharts/modules/item-series", ["highcharts"], function (d) { b(d); b.Highcharts = d; return b }) : b("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (b) {
    function d(b, d, c, C) { b.hasOwnProperty(d) || (b[d] = C.apply(null, c)) } b = b ? b._modules : {}; d(b, "modules/item-series.src.js", [b["parts/Globals.js"], b["parts/Options.js"], b["parts/Utilities.js"]], function (b, d, c) {
        var C = c.defined,
        F = c.extend, G = c.fireEvent, D = c.isNumber, H = c.merge, I = c.objectEach, J = c.pick; c = c.seriesType; var m = b.seriesTypes.pie.prototype.pointClass.prototype; c("item", "pie", { endAngle: void 0, innerSize: "40%", itemPadding: .1, layout: "vertical", marker: H(d.defaultOptions.plotOptions.line.marker, { radius: null }), rows: void 0, crisp: !1, showInLegend: !0, startAngle: void 0 }, {
            markerAttribs: void 0, translate: function (a) {
                0 === this.total && (this.center = this.getCenter()); this.slots || (this.slots = []); D(this.options.startAngle) && D(this.options.endAngle) ?
                    (b.seriesTypes.pie.prototype.translate.apply(this, arguments), this.slots = this.getSlots()) : (this.generatePoints(), G(this, "afterTranslate"))
            }, getSlots: function () {
                function a(a) { 0 < B && (a.row.colCount--, B--) } for (var b = this.center, c = b[2], d = b[3], x, n = this.slots, r, y, t, u, v, f, l, w, h = 0, p, z = this.endAngleRad - this.startAngleRad, q = Number.MAX_VALUE, A, e, k, g = this.options.rows, m = (c - d) / c, E = 0 === z % (2 * Math.PI); q > this.total + (e && E ? e.length : 0);)for (A = q, q = n.length = 0, e = k, k = [], h++, p = c / h / 2, g ? (d = (p - g) / p * c, 0 <= d ? p = g : (d = 0, m = 1)) : p = Math.floor(p *
                    m), x = p; 0 < x; x--)t = (d + x / p * (c - d - h)) / 2, u = z * t, v = Math.ceil(u / h), k.push({ rowRadius: t, rowLength: u, colCount: v }), q += v + 1; if (e) {
                        for (var B = A - this.total - (E ? e.length : 0); 0 < B;)e.map(function (a) { return { angle: a.colCount / a.rowLength, row: a } }).sort(function (a, b) { return b.angle - a.angle }).slice(0, Math.min(B, Math.ceil(e.length / 2))).forEach(a); e.forEach(function (a) { var c = a.rowRadius; f = (a = a.colCount) ? z / a : 0; for (w = 0; w <= a; w += 1)l = this.startAngleRad + w * f, r = b[0] + Math.cos(l) * c, y = b[1] + Math.sin(l) * c, n.push({ x: r, y: y, angle: l }) }, this);
                        n.sort(function (a, b) { return a.angle - b.angle }); this.itemSize = h; return n
                    }
            }, getRows: function () { var a = this.options.rows; if (!a) { var b = this.chart.plotWidth / this.chart.plotHeight; a = Math.sqrt(this.total); if (1 < b) for (a = Math.ceil(a); 0 < a;) { var c = this.total / a; if (c / a > b) break; a-- } else for (a = Math.floor(a); a < this.total;) { c = this.total / a; if (c / a < b) break; a++ } } return a }, drawPoints: function () {
                var a = this, b = this.options, c = a.chart.renderer, d = b.marker, m = this.borderWidth % 2 ? .5 : 1, n = 0, r = this.getRows(), y = Math.ceil(this.total / r),
                t = this.chart.plotWidth / y, u = this.chart.plotHeight / r, v = this.itemSize || Math.min(t, u); this.points.forEach(function (f) {
                    var l, w, h = f.marker || {}, p = h.symbol || d.symbol; h = J(h.radius, d.radius); var z = C(h) ? 2 * h : v, q = z * b.itemPadding, A; f.graphics = l = f.graphics || {}; a.chart.styledMode || (w = a.pointAttribs(f, f.selected && "select")); if (!f.isNull && f.visible) {
                        f.graphic || (f.graphic = c.g("point").add(a.group)); for (var e = 0; e < f.y; e++) {
                            if (a.center && a.slots) { var k = a.slots.shift(); var g = k.x - v / 2; k = k.y - v / 2 } else "horizontal" === b.layout ?
                                (g = n % y * t, k = u * Math.floor(n / y)) : (g = t * Math.floor(n / r), k = n % r * u); g += q; k += q; var x = A = Math.round(z - 2 * q); a.options.crisp && (g = Math.round(g) - m, k = Math.round(k) + m); g = { x: g, y: k, width: A, height: x }; "undefined" !== typeof h && (g.r = h); l[e] ? l[e].animate(g) : l[e] = c.symbol(p, null, null, null, null, { backgroundSize: "within" }).attr(F(g, w)).add(f.graphic); l[e].isActive = !0; n++
                        }
                    } I(l, function (a, b) { a.isActive ? a.isActive = !1 : (a.destroy(), delete l[b]) })
                })
            }, drawDataLabels: function () {
                this.center && this.slots ? b.seriesTypes.pie.prototype.drawDataLabels.call(this) :
                this.points.forEach(function (a) { a.destroyElements({ dataLabel: 1 }) })
            }, animate: function (a) { a ? this.group.attr({ opacity: 0 }) : this.group.animate({ opacity: 1 }, this.options.animation) }
        }, { connectorShapes: m.connectorShapes, getConnectorPath: m.getConnectorPath, setVisible: m.setVisible, getTranslate: m.getTranslate }); ""
    }); d(b, "masters/modules/item-series.src.js", [], function () { })
});
//# sourceMappingURL=item-series.js.map