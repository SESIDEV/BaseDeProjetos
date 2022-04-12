/*
 Highstock JS v8.1.2 (2020-06-16)

 Advanced Highstock tools

 (c) 2010-2019 Highsoft AS
 Author: Torstein Honsi

 License: www.highcharts.com/license
*/
(function (d) { "object" === typeof module && module.exports ? (d["default"] = d, module.exports = d) : "function" === typeof define && define.amd ? define("highcharts/modules/stock-tools", ["highcharts", "highcharts/modules/stock"], function (p) { d(p); d.Highcharts = p; return d }) : d("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (d) {
    function p(d, e, l, k) { d.hasOwnProperty(e) || (d[e] = k.apply(null, l)) } d = d ? d._modules : {}; p(d, "annotations/eventEmitterMixin.js", [d["parts/Globals.js"], d["parts/Utilities.js"]], function (d, e) {
        var l =
            e.addEvent, k = e.fireEvent, n = e.inArray, c = e.objectEach, m = e.pick, C = e.removeEvent; return {
                addEvents: function () {
                    var b = this, h = function (h) { l(h, d.isTouchDevice ? "touchstart" : "mousedown", function (g) { b.onMouseDown(g) }) }; h(this.graphic.element); (b.labels || []).forEach(function (b) { b.options.useHTML && b.graphic.text && h(b.graphic.text.element) }); c(b.options.events, function (h, g) {
                        var a = function (a) { "click" === g && b.cancelClick || h.call(b, b.chart.pointer.normalize(a), b.target) }; if (-1 === n(g, b.nonDOMEvents || [])) b.graphic.on(g,
                            a); else l(b, g, a)
                    }); if (b.options.draggable && (l(b, d.isTouchDevice ? "touchmove" : "drag", b.onDrag), !b.graphic.renderer.styledMode)) { var q = { cursor: { x: "ew-resize", y: "ns-resize", xy: "move" }[b.options.draggable] }; b.graphic.css(q); (b.labels || []).forEach(function (b) { b.options.useHTML && b.graphic.text && b.graphic.text.css(q) }) } b.isUpdating || k(b, "add")
                }, removeDocEvents: function () { this.removeDrag && (this.removeDrag = this.removeDrag()); this.removeMouseUp && (this.removeMouseUp = this.removeMouseUp()) }, onMouseDown: function (b) {
                    var h =
                        this, c = h.chart.pointer; b.preventDefault && b.preventDefault(); if (2 !== b.button) {
                            b = c.normalize(b); var u = b.chartX; var g = b.chartY; h.cancelClick = !1; h.chart.hasDraggedAnnotation = !0; h.removeDrag = l(d.doc, d.isTouchDevice ? "touchmove" : "mousemove", function (a) { h.hasDragged = !0; a = c.normalize(a); a.prevChartX = u; a.prevChartY = g; k(h, "drag", a); u = a.chartX; g = a.chartY }); h.removeMouseUp = l(d.doc, d.isTouchDevice ? "touchend" : "mouseup", function (a) {
                                h.cancelClick = h.hasDragged; h.hasDragged = !1; h.chart.hasDraggedAnnotation = !1; k(m(h.target,
                                    h), "afterUpdate"); h.onMouseUp(a)
                            })
                        }
                }, onMouseUp: function (b) { var h = this.chart; b = this.target || this; var c = h.options.annotations; h = h.annotations.indexOf(b); this.removeDocEvents(); c[h] = b.options }, onDrag: function (b) {
                    if (this.chart.isInsidePlot(b.chartX - this.chart.plotLeft, b.chartY - this.chart.plotTop)) {
                        var h = this.mouseMoveToTranslation(b); "x" === this.options.draggable && (h.y = 0); "y" === this.options.draggable && (h.x = 0); this.points.length ? this.translate(h.x, h.y) : (this.shapes.forEach(function (b) { b.translate(h.x, h.y) }),
                            this.labels.forEach(function (b) { b.translate(h.x, h.y) })); this.redraw(!1)
                    }
                }, mouseMoveToRadians: function (b, h, c) { var m = b.prevChartY - c, g = b.prevChartX - h; c = b.chartY - c; b = b.chartX - h; this.chart.inverted && (h = g, g = m, m = h, h = b, b = c, c = h); return Math.atan2(c, b) - Math.atan2(m, g) }, mouseMoveToTranslation: function (b) { var h = b.chartX - b.prevChartX; b = b.chartY - b.prevChartY; if (this.chart.inverted) { var c = b; b = h; h = c } return { x: h, y: b } }, mouseMoveToScale: function (b, h, c) {
                    h = (b.chartX - h || 1) / (b.prevChartX - h || 1); b = (b.chartY - c || 1) / (b.prevChartY -
                        c || 1); this.chart.inverted && (c = b, b = h, h = c); return { x: h, y: b }
                }, destroy: function () { this.removeDocEvents(); C(this); this.hcEvents = null }
            }
    }); p(d, "annotations/ControlPoint.js", [d["parts/Utilities.js"], d["annotations/eventEmitterMixin.js"]], function (d, e) {
        var l = d.merge, k = d.pick; return function () {
            function d(c, d, n, b) {
                this.addEvents = e.addEvents; this.graphic = void 0; this.mouseMoveToRadians = e.mouseMoveToRadians; this.mouseMoveToScale = e.mouseMoveToScale; this.mouseMoveToTranslation = e.mouseMoveToTranslation; this.onDrag =
                    e.onDrag; this.onMouseDown = e.onMouseDown; this.onMouseUp = e.onMouseUp; this.removeDocEvents = e.removeDocEvents; this.nonDOMEvents = ["drag"]; this.chart = c; this.target = d; this.options = n; this.index = k(n.index, b)
            } d.prototype.setVisibility = function (c) { this.graphic.attr("visibility", c ? "visible" : "hidden"); this.options.visible = c }; d.prototype.render = function () {
                var c = this.chart, d = this.options; this.graphic = c.renderer.symbol(d.symbol, 0, 0, d.width, d.height).add(c.controlPointsGroup).css(d.style); this.setVisibility(d.visible);
                this.addEvents()
            }; d.prototype.redraw = function (c) { this.graphic[c ? "animate" : "attr"](this.options.positioner.call(this, this.target)) }; d.prototype.destroy = function () { e.destroy.call(this); this.graphic && (this.graphic = this.graphic.destroy()); this.options = this.target = this.chart = null }; d.prototype.update = function (c) { var d = this.chart, e = this.target, b = this.index; c = l(!0, this.options, c); this.destroy(); this.constructor(d, e, c, b); this.render(d.controlPointsGroup); this.redraw() }; return d
        }()
    }); p(d, "annotations/MockPoint.js",
        [d["parts/Globals.js"], d["parts/Utilities.js"]], function (d, e) {
            var l = e.defined, k = e.fireEvent; return function () {
                function e(c, e, k) { this.y = this.x = this.plotY = this.plotX = this.isInside = void 0; this.mock = !0; this.series = { visible: !0, chart: c, getPlotBox: d.Series.prototype.getPlotBox }; this.target = e || null; this.options = k; this.applyOptions(this.getOptions()) } e.fromPoint = function (c) { return new e(c.series.chart, null, { x: c.x, y: c.y, xAxis: c.series.xAxis, yAxis: c.series.yAxis }) }; e.pointToPixels = function (c, d) {
                    var e = c.series,
                    b = e.chart, h = c.plotX, m = c.plotY; b.inverted && (c.mock ? (h = c.plotY, m = c.plotX) : (h = b.plotWidth - c.plotY, m = b.plotHeight - c.plotX)); e && !d && (c = e.getPlotBox(), h += c.translateX, m += c.translateY); return { x: h, y: m }
                }; e.pointToOptions = function (c) { return { x: c.x, y: c.y, xAxis: c.series.xAxis, yAxis: c.series.yAxis } }; e.prototype.hasDynamicOptions = function () { return "function" === typeof this.options }; e.prototype.getOptions = function () { return this.hasDynamicOptions() ? this.options(this.target) : this.options }; e.prototype.applyOptions = function (c) {
                    this.command =
                    c.command; this.setAxis(c, "x"); this.setAxis(c, "y"); this.refresh()
                }; e.prototype.setAxis = function (c, e) { e += "Axis"; c = c[e]; var m = this.series.chart; this.series[e] = c instanceof d.Axis ? c : l(c) ? m[e][c] || m.get(c) : null }; e.prototype.toAnchor = function () { var c = [this.plotX, this.plotY, 0, 0]; this.series.chart.inverted && (c[0] = this.plotY, c[1] = this.plotX); return c }; e.prototype.getLabelConfig = function () { return { x: this.x, y: this.y, point: this } }; e.prototype.isInsidePlot = function () {
                    var c = this.plotX, d = this.plotY, e = this.series.xAxis,
                    b = this.series.yAxis, h = { x: c, y: d, isInsidePlot: !0 }; e && (h.isInsidePlot = l(c) && 0 <= c && c <= e.len); b && (h.isInsidePlot = h.isInsidePlot && l(d) && 0 <= d && d <= b.len); k(this.series.chart, "afterIsInsidePlot", h); return h.isInsidePlot
                }; e.prototype.refresh = function () { var c = this.series, d = c.xAxis; c = c.yAxis; var e = this.getOptions(); d ? (this.x = e.x, this.plotX = d.toPixels(e.x, !0)) : (this.x = null, this.plotX = e.x); c ? (this.y = e.y, this.plotY = c.toPixels(e.y, !0)) : (this.y = null, this.plotY = e.y); this.isInside = this.isInsidePlot() }; e.prototype.translate =
                    function (c, d, e, b) { this.hasDynamicOptions() || (this.plotX += e, this.plotY += b, this.refreshOptions()) }; e.prototype.scale = function (c, d, e, b) { if (!this.hasDynamicOptions()) { var h = this.plotY * b; this.plotX = (1 - e) * c + this.plotX * e; this.plotY = (1 - b) * d + h; this.refreshOptions() } }; e.prototype.rotate = function (c, d, e) { if (!this.hasDynamicOptions()) { var b = Math.cos(e); e = Math.sin(e); var h = this.plotX, k = this.plotY; h -= c; k -= d; this.plotX = h * b - k * e + c; this.plotY = h * e + k * b + d; this.refreshOptions() } }; e.prototype.refreshOptions = function () {
                        var c =
                            this.series, e = c.xAxis; c = c.yAxis; this.x = this.options.x = e ? this.options.x = e.toValue(this.plotX, !0) : this.plotX; this.y = this.options.y = c ? c.toValue(this.plotY, !0) : this.plotY
                    }; return e
            }()
        }); p(d, "annotations/controllable/controllableMixin.js", [d["annotations/ControlPoint.js"], d["annotations/MockPoint.js"], d["parts/Tooltip.js"], d["parts/Utilities.js"]], function (d, e, l, k) {
            var n = k.isObject, c = k.isString, m = k.merge, z = k.splat; return {
                init: function (b, h, c) {
                    this.annotation = b; this.chart = b.chart; this.options = h; this.points =
                        []; this.controlPoints = []; this.index = c; this.linkPoints(); this.addControlPoints()
                }, attr: function () { this.graphic.attr.apply(this.graphic, arguments) }, getPointsOptions: function () { var b = this.options; return b.points || b.point && z(b.point) }, attrsFromOptions: function (b) { var h = this.constructor.attrsMap, c = {}, e, g = this.chart.styledMode; for (e in b) { var a = h[e]; !a || g && -1 !== ["fill", "stroke", "stroke-width"].indexOf(a) || (c[a] = b[e]) } return c }, anchor: function (b) {
                    var c = b.series.getPlotBox(); b = b.mock ? b.toAnchor() : l.prototype.getAnchor.call({ chart: b.series.chart },
                        b); b = { x: b[0] + (this.options.x || 0), y: b[1] + (this.options.y || 0), height: b[2] || 0, width: b[3] || 0 }; return { relativePosition: b, absolutePosition: m(b, { x: b.x + c.translateX, y: b.y + c.translateY }) }
                }, point: function (b, h) { if (b && b.series) return b; h && null !== h.series || (n(b) ? h = new e(this.chart, this, b) : c(b) ? h = this.chart.get(b) || null : "function" === typeof b && (h = b.call(h, this), h = h.series ? h : new e(this.chart, this, b))); return h }, linkPoints: function () {
                    var b = this.getPointsOptions(), c = this.points, e = b && b.length || 0, d; for (d = 0; d < e; d++) {
                        var g =
                            this.point(b[d], c[d]); if (!g) { c.length = 0; return } g.mock && g.refresh(); c[d] = g
                    } return c
                }, addControlPoints: function () { var b = this.options.controlPoints; (b || []).forEach(function (c, e) { c = m(this.options.controlPointOptions, c); c.index || (c.index = e); b[e] = c; this.controlPoints.push(new d(this.chart, this, c)) }, this) }, shouldBeDrawn: function () { return !!this.points.length }, render: function (b) { this.controlPoints.forEach(function (b) { b.render() }) }, redraw: function (b) { this.controlPoints.forEach(function (c) { c.redraw(b) }) }, transform: function (b,
                    c, e, d, g) { if (this.chart.inverted) { var a = c; c = e; e = a } this.points.forEach(function (a, v) { this.transformPoint(b, c, e, d, g, v) }, this) }, transformPoint: function (b, c, d, k, g, a) { var f = this.points[a]; f.mock || (f = this.points[a] = e.fromPoint(f)); f[b](c, d, k, g) }, translate: function (b, c) { this.transform("translate", null, null, b, c) }, translatePoint: function (b, c, e) { this.transformPoint("translate", null, null, b, c, e) }, translateShape: function (b, c) {
                        var e = this.annotation.chart, d = this.annotation.userOptions, g = e.annotations.indexOf(this.annotation);
                        e = e.options.annotations[g]; this.translatePoint(b, c, 0); e[this.collection][this.index].point = this.options.point; d[this.collection][this.index].point = this.options.point
                    }, rotate: function (b, c, e) { this.transform("rotate", b, c, e) }, scale: function (b, c, e, d) { this.transform("scale", b, c, e, d) }, setControlPointsVisibility: function (b) { this.controlPoints.forEach(function (c) { c.setVisibility(b) }) }, destroy: function () {
                        this.graphic && (this.graphic = this.graphic.destroy()); this.tracker && (this.tracker = this.tracker.destroy());
                        this.controlPoints.forEach(function (b) { b.destroy() }); this.options = this.controlPoints = this.points = this.chart = null; this.annotation && (this.annotation = null)
                    }, update: function (b) { var c = this.annotation; b = m(!0, this.options, b); var e = this.graphic.parentGroup; this.destroy(); this.constructor(c, b); this.render(e); this.redraw() }
            }
        }); p(d, "annotations/controllable/markerMixin.js", [d["parts/Globals.js"], d["parts/Utilities.js"]], function (d, e) {
            var l = e.addEvent, k = e.defined, n = e.merge, c = e.objectEach, m = e.uniqueKey, z = {
                arrow: {
                    tagName: "marker",
                    render: !1, id: "arrow", refY: 5, refX: 9, markerWidth: 10, markerHeight: 10, children: [{ tagName: "path", d: "M 0 0 L 10 5 L 0 10 Z", strokeWidth: 0 }]
                }, "reverse-arrow": { tagName: "marker", render: !1, id: "reverse-arrow", refY: 5, refX: 1, markerWidth: 10, markerHeight: 10, children: [{ tagName: "path", d: "M 0 5 L 10 0 L 10 10 Z", strokeWidth: 0 }] }
            }; d.SVGRenderer.prototype.addMarker = function (b, c) {
                var e = { id: b }, d = { stroke: c.color || "none", fill: c.color || "rgba(0, 0, 0, 0.75)" }; e.children = c.children.map(function (b) { return n(d, b) }); c = this.definition(n(!0,
                    { markerWidth: 20, markerHeight: 20, refX: 0, refY: 0, orient: "auto" }, c, e)); c.id = b; return c
            }; e = function (b) { return function (c) { this.attr(b, "url(#" + c + ")") } }; e = {
                markerEndSetter: e("marker-end"), markerStartSetter: e("marker-start"), setItemMarkers: function (b) {
                    var c = b.options, e = b.chart, d = e.options.defs, g = c.fill, a = k(g) && "none" !== g ? g : c.stroke;["markerStart", "markerEnd"].forEach(function (f) {
                        var g = c[f], r; if (g) {
                            for (r in d) { var h = d[r]; if (g === h.id && "marker" === h.tagName) { var t = h; break } } t && (g = b[f] = e.renderer.addMarker((c.id ||
                                m()) + "-" + t.id, n(t, { color: a })), b.attr(f, g.attr("id")))
                        }
                    })
                }
            }; l(d.Chart, "afterGetContainer", function () { this.options.defs = n(z, this.options.defs || {}); c(this.options.defs, function (b) { "marker" === b.tagName && !1 !== b.render && this.renderer.addMarker(b.id, b) }, this) }); return e
        }); p(d, "annotations/controllable/ControllablePath.js", [d["annotations/controllable/controllableMixin.js"], d["parts/Globals.js"], d["annotations/controllable/markerMixin.js"], d["parts/Utilities.js"]], function (d, e, l, k) {
            var n = k.extend; k = k.merge;
            var c = "rgba(192,192,192," + (e.svg ? .0001 : .002) + ")"; e = function (c, e, b) { this.init(c, e, b); this.collection = "shapes" }; e.attrsMap = { dashStyle: "dashstyle", strokeWidth: "stroke-width", stroke: "stroke", fill: "fill", zIndex: "zIndex" }; k(!0, e.prototype, d, {
                type: "path", setMarkers: l.setItemMarkers, toD: function () {
                    var c = this.options.d; if (c) return "function" === typeof c ? c.call(this) : c; c = this.points; var e = c.length, b = e, d = c[0], k = b && this.anchor(d).absolutePosition, u = 0, g = []; if (k) for (g.push(["M", k.x, k.y]); ++u < e && b;)d = c[u], b = d.command ||
                        "L", k = this.anchor(d).absolutePosition, "M" === b ? g.push([b, k.x, k.y]) : "L" === b ? g.push([b, k.x, k.y]) : "Z" === b && g.push([b]), b = d.series.visible; return b ? this.chart.renderer.crispLine(g, this.graphic.strokeWidth()) : null
                }, shouldBeDrawn: function () { return d.shouldBeDrawn.call(this) || !!this.options.d }, render: function (e) {
                    var k = this.options, b = this.attrsFromOptions(k); this.graphic = this.annotation.chart.renderer.path([["M", 0, 0]]).attr(b).add(e); k.className && this.graphic.addClass(k.className); this.tracker = this.annotation.chart.renderer.path([["M",
                        0, 0]]).addClass("highcharts-tracker-line").attr({ zIndex: 2 }).add(e); this.annotation.chart.styledMode || this.tracker.attr({ "stroke-linejoin": "round", stroke: c, fill: c, "stroke-width": this.graphic.strokeWidth() + 2 * k.snap }); d.render.call(this); n(this.graphic, { markerStartSetter: l.markerStartSetter, markerEndSetter: l.markerEndSetter }); this.setMarkers(this)
                }, redraw: function (c) {
                    var e = this.toD(), b = c ? "animate" : "attr"; e ? (this.graphic[b]({ d: e }), this.tracker[b]({ d: e })) : (this.graphic.attr({ d: "M 0 -9000000000" }), this.tracker.attr({ d: "M 0 -9000000000" }));
                    this.graphic.placed = this.tracker.placed = !!e; d.redraw.call(this, c)
                }
            }); return e
        }); p(d, "annotations/controllable/ControllableRect.js", [d["annotations/controllable/controllableMixin.js"], d["annotations/controllable/ControllablePath.js"], d["parts/Utilities.js"]], function (d, e, l) {
            l = l.merge; var k = function (e, c, d) { this.init(e, c, d); this.collection = "shapes" }; k.attrsMap = l(e.attrsMap, { width: "width", height: "height" }); l(!0, k.prototype, d, {
                type: "rect", translate: d.translateShape, render: function (e) {
                    var c = this.attrsFromOptions(this.options);
                    this.graphic = this.annotation.chart.renderer.rect(0, -9E9, 0, 0).attr(c).add(e); d.render.call(this)
                }, redraw: function (e) { var c = this.anchor(this.points[0]).absolutePosition; if (c) this.graphic[e ? "animate" : "attr"]({ x: c.x, y: c.y, width: this.options.width, height: this.options.height }); else this.attr({ x: 0, y: -9E9 }); this.graphic.placed = !!c; d.redraw.call(this, e) }
            }); return k
        }); p(d, "annotations/controllable/ControllableCircle.js", [d["annotations/controllable/controllableMixin.js"], d["annotations/controllable/ControllablePath.js"],
        d["parts/Utilities.js"]], function (d, e, l) {
            l = l.merge; var k = function (e, c, d) { this.init(e, c, d); this.collection = "shapes" }; k.attrsMap = l(e.attrsMap, { r: "r" }); l(!0, k.prototype, d, {
                type: "circle", translate: d.translateShape, render: function (e) { var c = this.attrsFromOptions(this.options); this.graphic = this.annotation.chart.renderer.circle(0, -9E9, 0).attr(c).add(e); d.render.call(this) }, redraw: function (e) {
                    var c = this.anchor(this.points[0]).absolutePosition; if (c) this.graphic[e ? "animate" : "attr"]({ x: c.x, y: c.y, r: this.options.r });
                    else this.graphic.attr({ x: 0, y: -9E9 }); this.graphic.placed = !!c; d.redraw.call(this, e)
                }, setRadius: function (e) { this.options.r = e }
            }); return k
        }); p(d, "annotations/controllable/ControllableLabel.js", [d["annotations/controllable/controllableMixin.js"], d["parts/Globals.js"], d["annotations/MockPoint.js"], d["parts/Tooltip.js"], d["parts/Utilities.js"]], function (d, e, l, k, n) {
            var c = n.extend, m = n.format, z = n.isNumber, b = n.merge, h = n.pick, q = function (b, c, a) { this.init(b, c, a); this.collection = "labels" }; q.shapesWithoutBackground =
                ["connector"]; q.alignedPosition = function (b, c) { var a = b.align, f = b.verticalAlign, e = (c.x || 0) + (b.x || 0), d = (c.y || 0) + (b.y || 0), g, h; "right" === a ? g = 1 : "center" === a && (g = 2); g && (e += (c.width - (b.width || 0)) / g); "bottom" === f ? h = 1 : "middle" === f && (h = 2); h && (d += (c.height - (b.height || 0)) / h); return { x: Math.round(e), y: Math.round(d) } }; q.justifiedOptions = function (b, c, a, f) {
                    var e = a.align, d = a.verticalAlign, g = c.box ? 0 : c.padding || 0, h = c.getBBox(); c = { align: e, verticalAlign: d, x: a.x, y: a.y, width: c.width, height: c.height }; a = f.x - b.plotLeft; var w =
                        f.y - b.plotTop; f = a + g; 0 > f && ("right" === e ? c.align = "left" : c.x = -f); f = a + h.width - g; f > b.plotWidth && ("left" === e ? c.align = "right" : c.x = b.plotWidth - f); f = w + g; 0 > f && ("bottom" === d ? c.verticalAlign = "top" : c.y = -f); f = w + h.height - g; f > b.plotHeight && ("top" === d ? c.verticalAlign = "bottom" : c.y = b.plotHeight - f); return c
                }; q.attrsMap = { backgroundColor: "fill", borderColor: "stroke", borderWidth: "stroke-width", zIndex: "zIndex", borderRadius: "r", padding: "padding" }; b(!0, q.prototype, d, {
                    translatePoint: function (c, b) {
                        d.translatePoint.call(this, c,
                            b, 0)
                    }, translate: function (c, b) { var a = this.annotation.chart, f = this.annotation.userOptions, e = a.annotations.indexOf(this.annotation); e = a.options.annotations[e]; a.inverted && (a = c, c = b, b = a); this.options.x += c; this.options.y += b; e[this.collection][this.index].x = this.options.x; e[this.collection][this.index].y = this.options.y; f[this.collection][this.index].x = this.options.x; f[this.collection][this.index].y = this.options.y }, render: function (c) {
                        var b = this.options, a = this.attrsFromOptions(b), f = b.style; this.graphic = this.annotation.chart.renderer.label("",
                            0, -9999, b.shape, null, null, b.useHTML, null, "annotation-label").attr(a).add(c); this.annotation.chart.styledMode || ("contrast" === f.color && (f.color = this.annotation.chart.renderer.getContrast(-1 < q.shapesWithoutBackground.indexOf(b.shape) ? "#FFFFFF" : b.backgroundColor)), this.graphic.css(b.style).shadow(b.shadow)); b.className && this.graphic.addClass(b.className); this.graphic.labelrank = b.labelrank; d.render.call(this)
                    }, redraw: function (b) {
                        var c = this.options, a = this.text || c.format || c.text, f = this.graphic, e = this.points[0];
                        f.attr({ text: a ? m(a, e.getLabelConfig(), this.annotation.chart) : c.formatter.call(e, this) }); c = this.anchor(e); (a = this.position(c)) ? (f.alignAttr = a, a.anchorX = c.absolutePosition.x, a.anchorY = c.absolutePosition.y, f[b ? "animate" : "attr"](a)) : f.attr({ x: 0, y: -9999 }); f.placed = !!a; d.redraw.call(this, b)
                    }, anchor: function () { var b = d.anchor.apply(this, arguments), c = this.options.x || 0, a = this.options.y || 0; b.absolutePosition.x -= c; b.absolutePosition.y -= a; b.relativePosition.x -= c; b.relativePosition.y -= a; return b }, position: function (b) {
                        var e =
                            this.graphic, a = this.annotation.chart, f = this.points[0], d = this.options, r = b.absolutePosition, x = b.relativePosition; if (b = f.series.visible && l.prototype.isInsidePlot.call(f)) {
                                if (d.distance) var t = k.prototype.getPosition.call({ chart: a, distance: h(d.distance, 16) }, e.width, e.height, { plotX: x.x, plotY: x.y, negative: f.negative, ttBelow: f.ttBelow, h: x.height || x.width }); else d.positioner ? t = d.positioner.call(this) : (f = { x: r.x, y: r.y, width: 0, height: 0 }, t = q.alignedPosition(c(d, { width: e.width, height: e.height }), f), "justify" ===
                                    this.options.overflow && (t = q.alignedPosition(q.justifiedOptions(a, e, d, t), f))); d.crop && (d = t.x - a.plotLeft, f = t.y - a.plotTop, b = a.isInsidePlot(d, f) && a.isInsidePlot(d + e.width, f + e.height))
                            } return b ? t : null
                    }
                }); e.SVGRenderer.prototype.symbols.connector = function (b, c, a, f, e) {
                    var d = e && e.anchorX; e = e && e.anchorY; var g = a / 2; if (z(d) && z(e)) { var h = [["M", d, e]]; var w = c - e; 0 > w && (w = -f - w); w < a && (g = d < b + a / 2 ? w : a - w); e > c + f ? h.push(["L", b + g, c + f]) : e < c ? h.push(["L", b + g, c]) : d < b ? h.push(["L", b, c + f / 2]) : d > b + a && h.push(["L", b + a, c + f / 2]) } return h ||
                        []
                }; return q
        }); p(d, "annotations/controllable/ControllableImage.js", [d["annotations/controllable/ControllableLabel.js"], d["annotations/controllable/controllableMixin.js"], d["parts/Utilities.js"]], function (d, e, l) {
            l = l.merge; var k = function (e, c, d) { this.init(e, c, d); this.collection = "shapes" }; k.attrsMap = { width: "width", height: "height", zIndex: "zIndex" }; l(!0, k.prototype, e, {
                type: "image", translate: e.translateShape, render: function (d) {
                    var c = this.attrsFromOptions(this.options), k = this.options; this.graphic = this.annotation.chart.renderer.image(k.src,
                        0, -9E9, k.width, k.height).attr(c).add(d); this.graphic.width = k.width; this.graphic.height = k.height; e.render.call(this)
                }, redraw: function (k) { var c = this.anchor(this.points[0]); if (c = d.prototype.position.call(this, c)) this.graphic[k ? "animate" : "attr"]({ x: c.x, y: c.y }); else this.graphic.attr({ x: 0, y: -9E9 }); this.graphic.placed = !!c; e.redraw.call(this, k) }
            }); return k
        }); p(d, "annotations/annotations.src.js", [d["parts/Chart.js"], d["annotations/controllable/controllableMixin.js"], d["annotations/controllable/ControllableRect.js"],
        d["annotations/controllable/ControllableCircle.js"], d["annotations/controllable/ControllablePath.js"], d["annotations/controllable/ControllableImage.js"], d["annotations/controllable/ControllableLabel.js"], d["annotations/ControlPoint.js"], d["annotations/eventEmitterMixin.js"], d["parts/Globals.js"], d["annotations/MockPoint.js"], d["parts/Pointer.js"], d["parts/Utilities.js"]], function (d, e, l, k, n, c, m, p, b, h, q, u, g) {
            d = d.prototype; var a = g.addEvent, f = g.defined, v = g.destroyObjectProperties, r = g.erase, x = g.extend,
                t = g.find, w = g.fireEvent, y = g.merge, B = g.pick, D = g.splat; g = g.wrap; var A = function () {
                    function a(a, b) {
                        this.annotation = void 0; this.coll = "annotations"; this.shapesGroup = this.labelsGroup = this.labelCollector = this.group = this.graphic = this.collection = void 0; this.chart = a; this.points = []; this.controlPoints = []; this.coll = "annotations"; this.labels = []; this.shapes = []; this.options = y(this.defaultOptions, b); this.userOptions = b; b = this.getLabelsAndShapesOptions(this.options, b); this.options.labels = b.labels; this.options.shapes =
                            b.shapes; this.init(a, this.options)
                    } a.prototype.init = function () { this.linkPoints(); this.addControlPoints(); this.addShapes(); this.addLabels(); this.setLabelCollector() }; a.prototype.getLabelsAndShapesOptions = function (a, b) { var c = {};["labels", "shapes"].forEach(function (f) { a[f] && (c[f] = D(b[f]).map(function (b, c) { return y(a[f][c], b) })) }); return c }; a.prototype.addShapes = function () { (this.options.shapes || []).forEach(function (a, b) { a = this.initShape(a, b); y(!0, this.options.shapes[b], a.options) }, this) }; a.prototype.addLabels =
                        function () { (this.options.labels || []).forEach(function (a, b) { a = this.initLabel(a, b); y(!0, this.options.labels[b], a.options) }, this) }; a.prototype.addClipPaths = function () { this.setClipAxes(); this.clipXAxis && this.clipYAxis && (this.clipRect = this.chart.renderer.clipRect(this.getClipBox())) }; a.prototype.setClipAxes = function () {
                            var a = this.chart.xAxis, b = this.chart.yAxis, c = (this.options.labels || []).concat(this.options.shapes || []).reduce(function (c, f) {
                                return [a[f && f.point && f.point.xAxis] || c[0], b[f && f.point && f.point.yAxis] ||
                                    c[1]]
                            }, []); this.clipXAxis = c[0]; this.clipYAxis = c[1]
                        }; a.prototype.getClipBox = function () { if (this.clipXAxis && this.clipYAxis) return { x: this.clipXAxis.left, y: this.clipYAxis.top, width: this.clipXAxis.width, height: this.clipYAxis.height } }; a.prototype.setLabelCollector = function () { var a = this; a.labelCollector = function () { return a.labels.reduce(function (a, b) { b.options.allowOverlap || a.push(b.graphic); return a }, []) }; a.chart.labelCollectors.push(a.labelCollector) }; a.prototype.setOptions = function (a) {
                            this.options = y(this.defaultOptions,
                                a)
                        }; a.prototype.redraw = function (a) { this.linkPoints(); this.graphic || this.render(); this.clipRect && this.clipRect.animate(this.getClipBox()); this.redrawItems(this.shapes, a); this.redrawItems(this.labels, a); e.redraw.call(this, a) }; a.prototype.redrawItems = function (a, b) { for (var c = a.length; c--;)this.redrawItem(a[c], b) }; a.prototype.renderItems = function (a) { for (var b = a.length; b--;)this.renderItem(a[b]) }; a.prototype.render = function () {
                            var a = this.chart.renderer; this.graphic = a.g("annotation").attr({
                                zIndex: this.options.zIndex,
                                visibility: this.options.visible ? "visible" : "hidden"
                            }).add(); this.shapesGroup = a.g("annotation-shapes").add(this.graphic).clip(this.chart.plotBoxClip); this.labelsGroup = a.g("annotation-labels").attr({ translateX: 0, translateY: 0 }).add(this.graphic); this.addClipPaths(); this.clipRect && this.graphic.clip(this.clipRect); this.renderItems(this.shapes); this.renderItems(this.labels); this.addEvents(); e.render.call(this)
                        }; a.prototype.setVisibility = function (a) {
                            var b = this.options; a = B(a, !b.visible); this.graphic.attr("visibility",
                                a ? "visible" : "hidden"); a || this.setControlPointsVisibility(!1); b.visible = a
                        }; a.prototype.setControlPointsVisibility = function (a) { var b = function (b) { b.setControlPointsVisibility(a) }; e.setControlPointsVisibility.call(this, a); this.shapes.forEach(b); this.labels.forEach(b) }; a.prototype.destroy = function () {
                            var a = this.chart, c = function (a) { a.destroy() }; this.labels.forEach(c); this.shapes.forEach(c); this.clipYAxis = this.clipXAxis = null; r(a.labelCollectors, this.labelCollector); b.destroy.call(this); e.destroy.call(this);
                            v(this, a)
                        }; a.prototype.remove = function () { return this.chart.removeAnnotation(this) }; a.prototype.update = function (a, b) { var c = this.chart, f = this.getLabelsAndShapesOptions(this.userOptions, a), e = c.annotations.indexOf(this); a = y(!0, this.userOptions, a); a.labels = f.labels; a.shapes = f.shapes; this.destroy(); this.constructor(c, a); c.options.annotations[e] = a; this.isUpdating = !0; B(b, !0) && c.redraw(); w(this, "afterUpdate"); this.isUpdating = !1 }; a.prototype.initShape = function (b, c) {
                            b = y(this.options.shapeOptions, { controlPointOptions: this.options.controlPointOptions },
                                b); c = new a.shapesMap[b.type](this, b, c); c.itemType = "shape"; this.shapes.push(c); return c
                        }; a.prototype.initLabel = function (a, b) { a = y(this.options.labelOptions, { controlPointOptions: this.options.controlPointOptions }, a); b = new m(this, a, b); b.itemType = "label"; this.labels.push(b); return b }; a.prototype.redrawItem = function (a, b) { a.linkPoints(); a.shouldBeDrawn() ? (a.graphic || this.renderItem(a), a.redraw(B(b, !0) && a.graphic.placed), a.points.length && this.adjustVisibility(a)) : this.destroyItem(a) }; a.prototype.adjustVisibility =
                            function (a) { var b = !1, c = a.graphic; a.points.forEach(function (a) { !1 !== a.series.visible && !1 !== a.visible && (b = !0) }); b ? "hidden" === c.visibility && c.show() : c.hide() }; a.prototype.destroyItem = function (a) { r(this[a.itemType + "s"], a); a.destroy() }; a.prototype.renderItem = function (a) { a.render("label" === a.itemType ? this.labelsGroup : this.shapesGroup) }; a.ControlPoint = p; a.MockPoint = q; a.shapesMap = { rect: l, circle: k, path: n, image: c }; a.types = {}; return a
                }(); y(!0, A.prototype, e, b, y(A.prototype, {
                    nonDOMEvents: ["add", "afterUpdate",
                        "drag", "remove"], defaultOptions: {
                            visible: !0, draggable: "xy", labelOptions: { align: "center", allowOverlap: !1, backgroundColor: "rgba(0, 0, 0, 0.75)", borderColor: "black", borderRadius: 3, borderWidth: 1, className: "", crop: !1, formatter: function () { return f(this.y) ? this.y : "Annotation label" }, overflow: "justify", padding: 5, shadow: !1, shape: "callout", style: { fontSize: "11px", fontWeight: "normal", color: "contrast" }, useHTML: !1, verticalAlign: "bottom", x: 0, y: -16 }, shapeOptions: {
                                stroke: "rgba(0, 0, 0, 0.75)", strokeWidth: 1, fill: "rgba(0, 0, 0, 0.75)",
                                r: 0, snap: 2
                            }, controlPointOptions: { symbol: "circle", width: 10, height: 10, style: { stroke: "black", "stroke-width": 2, fill: "white" }, visible: !1, events: {} }, events: {}, zIndex: 6
                        }
                })); h.extendAnnotation = function (a, b, c, f) { b = b || A; y(!0, a.prototype, b.prototype, c); a.prototype.defaultOptions = y(a.prototype.defaultOptions, f || {}) }; x(d, {
                    initAnnotation: function (a) { a = new (A.types[a.type] || A)(this, a); this.annotations.push(a); return a }, addAnnotation: function (a, b) {
                        a = this.initAnnotation(a); this.options.annotations.push(a.options);
                        B(b, !0) && a.redraw(); return a
                    }, removeAnnotation: function (a) { var b = this.annotations, c = "annotations" === a.coll ? a : t(b, function (b) { return b.options.id === a }); c && (w(c, "remove"), r(this.options.annotations, c.options), r(b, c), c.destroy()) }, drawAnnotations: function () { this.plotBoxClip.attr(this.plotBox); this.annotations.forEach(function (a) { a.redraw() }) }
                }); d.collectionsWithUpdate.push("annotations"); d.collectionsWithInit.annotations = [d.addAnnotation]; d.callbacks.push(function (b) {
                    b.annotations = []; b.options.annotations ||
                        (b.options.annotations = []); b.plotBoxClip = this.renderer.clipRect(this.plotBox); b.controlPointsGroup = b.renderer.g("control-points").attr({ zIndex: 99 }).clip(b.plotBoxClip).add(); b.options.annotations.forEach(function (a, c) { a = b.initAnnotation(a); b.options.annotations[c] = a.options }); b.drawAnnotations(); a(b, "redraw", b.drawAnnotations); a(b, "destroy", function () { b.plotBoxClip.destroy(); b.controlPointsGroup.destroy() })
                }); g(u.prototype, "onContainerMouseDown", function (a) {
                    this.chart.hasDraggedAnnotation || a.apply(this,
                        Array.prototype.slice.call(arguments, 1))
                }); return h.Annotation = A
        }); p(d, "mixins/navigation.js", [], function () { return { initUpdate: function (d) { d.navigation || (d.navigation = { updates: [], update: function (e, d) { this.updates.forEach(function (k) { k.update.call(k.context, e, d) }) } }) }, addUpdate: function (d, e) { e.navigation || this.initUpdate(e); e.navigation.updates.push({ update: d, context: e }) } } }); p(d, "annotations/navigationBindings.js", [d["annotations/annotations.src.js"], d["mixins/navigation.js"], d["parts/Globals.js"],
        d["parts/Utilities.js"]], function (d, e, l, k) {
            function n(c) {
                var f = c.prototype.defaultOptions.events && c.prototype.defaultOptions.events.click; a(!0, c.prototype.defaultOptions.events, {
                    click: function (a) {
                        var c = this, e = c.chart.navigationBindings, d = e.activeAnnotation; f && f.call(c, a); d !== c ? (e.deselectAnnotation(), e.activeAnnotation = c, c.setControlPointsVisibility(!0), b(e, "showPopup", {
                            annotation: c, formType: "annotation-toolbar", options: e.annotationToFields(c), onSubmit: function (a) {
                                var b = {}; "remove" === a.actionType ?
                                    (e.activeAnnotation = !1, e.chart.removeAnnotation(c)) : (e.fieldsToOptions(a.fields, b), e.deselectAnnotation(), a = b.typeOptions, "measure" === c.options.type && (a.crosshairY.enabled = 0 !== a.crosshairY.strokeWidth, a.crosshairX.enabled = 0 !== a.crosshairX.strokeWidth), c.update(b))
                            }
                        })) : (e.deselectAnnotation(), b(e, "closePopup")); a.activeAnnotation = !0
                    }
                })
            } var c = k.addEvent, m = k.attr, p = k.format, b = k.fireEvent, h = k.isArray, q = k.isFunction, u = k.isNumber, g = k.isObject, a = k.merge, f = k.objectEach, v = k.pick; k = k.setOptions; var r = l.doc,
                x = l.win, t = function () {
                    function d(a, b) { this.selectedButton = this.boundClassNames = void 0; this.chart = a; this.options = b; this.eventsToUnbind = []; this.container = r.getElementsByClassName(this.options.bindingsClassName || "") } d.prototype.initEvents = function () {
                        var a = this, b = a.chart, d = a.container, e = a.options; a.boundClassNames = {}; f(e.bindings || {}, function (b) { a.boundClassNames[b.className] = b });[].forEach.call(d, function (b) {
                            a.eventsToUnbind.push(c(b, "click", function (c) {
                                var f = a.getButtonEvents(b, c); f && a.bindingsButtonClick(f.button,
                                    f.events, c)
                            }))
                        }); f(e.events || {}, function (b, f) { q(b) && a.eventsToUnbind.push(c(a, f, b)) }); a.eventsToUnbind.push(c(b.container, "click", function (c) { !b.cancelClick && b.isInsidePlot(c.chartX - b.plotLeft, c.chartY - b.plotTop) && a.bindingsChartClick(this, c) })); a.eventsToUnbind.push(c(b.container, l.isTouchDevice ? "touchmove" : "mousemove", function (b) { a.bindingsContainerMouseMove(this, b) }))
                    }; d.prototype.initUpdate = function () { var a = this; e.addUpdate(function (b) { a.update(b) }, this.chart) }; d.prototype.bindingsButtonClick =
                        function (a, c, f) { var d = this.chart; this.selectedButtonElement && (b(this, "deselectButton", { button: this.selectedButtonElement }), this.nextEvent && (this.currentUserDetails && "annotations" === this.currentUserDetails.coll && d.removeAnnotation(this.currentUserDetails), this.mouseMoveEvent = this.nextEvent = !1)); this.selectedButton = c; this.selectedButtonElement = a; b(this, "selectButton", { button: a }); c.init && c.init.call(this, a, f); (c.start || c.steps) && d.renderer.boxWrapper.addClass("highcharts-draw-mode") }; d.prototype.bindingsChartClick =
                            function (a, c) {
                                a = this.chart; var f = this.selectedButton; a = a.renderer.boxWrapper; var d; if (d = this.activeAnnotation && !c.activeAnnotation && c.target.parentNode) { a: { d = c.target; var e = x.Element.prototype, g = e.matches || e.msMatchesSelector || e.webkitMatchesSelector, h = null; if (e.closest) h = e.closest.call(d, ".highcharts-popup"); else { do { if (g.call(d, ".highcharts-popup")) break a; d = d.parentElement || d.parentNode } while (null !== d && 1 === d.nodeType) } d = h } d = !d } d && (b(this, "closePopup"), this.deselectAnnotation()); f && f.start && (this.nextEvent ?
                                    (this.nextEvent(c, this.currentUserDetails), this.steps && (this.stepIndex++, f.steps[this.stepIndex] ? this.mouseMoveEvent = this.nextEvent = f.steps[this.stepIndex] : (b(this, "deselectButton", { button: this.selectedButtonElement }), a.removeClass("highcharts-draw-mode"), f.end && f.end.call(this, c, this.currentUserDetails), this.mouseMoveEvent = this.nextEvent = !1, this.selectedButton = null))) : (this.currentUserDetails = f.start.call(this, c), f.steps ? (this.stepIndex = 0, this.steps = !0, this.mouseMoveEvent = this.nextEvent = f.steps[this.stepIndex]) :
                                        (b(this, "deselectButton", { button: this.selectedButtonElement }), a.removeClass("highcharts-draw-mode"), this.steps = !1, this.selectedButton = null, f.end && f.end.call(this, c, this.currentUserDetails))))
                            }; d.prototype.bindingsContainerMouseMove = function (a, b) { this.mouseMoveEvent && this.mouseMoveEvent(b, this.currentUserDetails) }; d.prototype.fieldsToOptions = function (a, b) {
                                f(a, function (a, c) {
                                    var f = parseFloat(a), d = c.split("."), e = b, g = d.length - 1; !u(f) || a.match(/px/g) || c.match(/format/g) || (a = f); "" !== a && "undefined" !== a &&
                                        d.forEach(function (b, c) { var f = v(d[c + 1], ""); g === c ? e[b] = a : (e[b] || (e[b] = f.match(/\d/g) ? [] : {}), e = e[b]) })
                                }); return b
                            }; d.prototype.deselectAnnotation = function () { this.activeAnnotation && (this.activeAnnotation.setControlPointsVisibility(!1), this.activeAnnotation = !1) }; d.prototype.annotationToFields = function (a) {
                                function b(c, d, e, r) {
                                    if (e && -1 === t.indexOf(d) && (0 <= (e.indexOf && e.indexOf(d)) || e[d] || !0 === e)) if (h(c)) r[d] = [], c.forEach(function (a, c) {
                                        g(a) ? (r[d][c] = {}, f(a, function (a, f) { b(a, f, k[d], r[d][c]) })) : b(a, 0, k[d],
                                            r[d])
                                    }); else if (g(c)) { var v = {}; h(r) ? (r.push(v), v[d] = {}, v = v[d]) : r[d] = v; f(c, function (a, c) { b(a, c, 0 === d ? e : k[d], v) }) } else "format" === d ? r[d] = [p(c, a.labels[0].points[0]).toString(), "text"] : h(r) ? r.push([c, w(c)]) : r[d] = [c, w(c)]
                                } var c = a.options, e = d.annotationsEditable, k = e.nestedOptions, w = this.utils.getFieldType, r = v(c.type, c.shapes && c.shapes[0] && c.shapes[0].type, c.labels && c.labels[0] && c.labels[0].itemType, "label"), t = d.annotationsNonEditable[c.langKey] || [], x = { langKey: c.langKey, type: r }; f(c, function (a, d) {
                                    "typeOptions" ===
                                    d ? (x[d] = {}, f(c[d], function (a, c) { b(a, c, k, x[d], !0) })) : b(a, d, e[r], x)
                                }); return x
                            }; d.prototype.getClickedClassNames = function (a, b) { var c = b.target; b = []; for (var f; c && ((f = m(c, "class")) && (b = b.concat(f.split(" ").map(function (a) { return [a, c] }))), c = c.parentNode, c !== a);); return b }; d.prototype.getButtonEvents = function (a, b) { var c = this, f; this.getClickedClassNames(a, b).forEach(function (a) { c.boundClassNames[a[0]] && !f && (f = { events: c.boundClassNames[a[0]], button: a[1] }) }); return f }; d.prototype.update = function (b) {
                                this.options =
                                a(!0, this.options, b); this.removeEvents(); this.initEvents()
                            }; d.prototype.removeEvents = function () { this.eventsToUnbind.forEach(function (a) { a() }) }; d.prototype.destroy = function () { this.removeEvents() }; d.annotationsEditable = {
                                nestedOptions: {
                                    labelOptions: ["style", "format", "backgroundColor"], labels: ["style"], label: ["style"], style: ["fontSize", "color"], background: ["fill", "strokeWidth", "stroke"], innerBackground: ["fill", "strokeWidth", "stroke"], outerBackground: ["fill", "strokeWidth", "stroke"], shapeOptions: ["fill",
                                        "strokeWidth", "stroke"], shapes: ["fill", "strokeWidth", "stroke"], line: ["strokeWidth", "stroke"], backgroundColors: [!0], connector: ["fill", "strokeWidth", "stroke"], crosshairX: ["strokeWidth", "stroke"], crosshairY: ["strokeWidth", "stroke"]
                                }, circle: ["shapes"], verticalLine: [], label: ["labelOptions"], measure: ["background", "crosshairY", "crosshairX"], fibonacci: [], tunnel: ["background", "line", "height"], pitchfork: ["innerBackground", "outerBackground"], rect: ["shapes"], crookedLine: [], basicAnnotation: []
                            }; d.annotationsNonEditable =
                                { rectangle: ["crosshairX", "crosshairY", "label"] }; return d
                }(); t.prototype.utils = { updateRectSize: function (a, b) { var c = b.chart, f = b.options.typeOptions, d = c.pointer.getCoordinates(a); a = d.xAxis[0].value - f.point.x; f = f.point.y - d.yAxis[0].value; b.update({ typeOptions: { background: { width: c.inverted ? f : a, height: c.inverted ? a : f } } }) }, getFieldType: function (a) { return { string: "text", number: "number", "boolean": "checkbox" }[typeof a] } }; l.Chart.prototype.initNavigationBindings = function () {
                    var a = this.options; a && a.navigation &&
                        a.navigation.bindings && (this.navigationBindings = new t(this, a.navigation), this.navigationBindings.initEvents(), this.navigationBindings.initUpdate())
                }; c(l.Chart, "load", function () { this.initNavigationBindings() }); c(l.Chart, "destroy", function () { this.navigationBindings && this.navigationBindings.destroy() }); c(t, "deselectButton", function () { this.selectedButtonElement = null }); c(d, "remove", function () { this.chart.navigationBindings && this.chart.navigationBindings.deselectAnnotation() }); l.Annotation && (n(d), f(d.types,
                    function (a) { n(a) })); k({
                        lang: {
                            navigation: {
                                popup: {
                                    simpleShapes: "Simple shapes", lines: "Lines", circle: "Circle", rectangle: "Rectangle", label: "Label", shapeOptions: "Shape options", typeOptions: "Details", fill: "Fill", format: "Text", strokeWidth: "Line width", stroke: "Line color", title: "Title", name: "Name", labelOptions: "Label options", labels: "Labels", backgroundColor: "Background color", backgroundColors: "Background colors", borderColor: "Border color", borderRadius: "Border radius", borderWidth: "Border width", style: "Style",
                                    padding: "Padding", fontSize: "Font size", color: "Color", height: "Height", shapes: "Shape options"
                                }
                            }
                        }, navigation: {
                            bindingsClassName: "highcharts-bindings-container", bindings: {
                                circleAnnotation: {
                                    className: "highcharts-circle-annotation", start: function (b) {
                                        b = this.chart.pointer.getCoordinates(b); var c = this.chart.options.navigation; return this.chart.addAnnotation(a({ langKey: "circle", type: "basicAnnotation", shapes: [{ type: "circle", point: { xAxis: 0, yAxis: 0, x: b.xAxis[0].value, y: b.yAxis[0].value }, r: 5 }] }, c.annotationsOptions,
                                            c.bindings.circleAnnotation.annotationsOptions))
                                    }, steps: [function (a, b) { var c = b.options.shapes[0].point, f = this.chart.xAxis[0].toPixels(c.x); c = this.chart.yAxis[0].toPixels(c.y); var d = this.chart.inverted; b.update({ shapes: [{ r: Math.max(Math.sqrt(Math.pow(d ? c - a.chartX : f - a.chartX, 2) + Math.pow(d ? f - a.chartY : c - a.chartY, 2)), 5) }] }) }]
                                }, rectangleAnnotation: {
                                    className: "highcharts-rectangle-annotation", start: function (b) {
                                        var c = this.chart.pointer.getCoordinates(b); b = this.chart.options.navigation; var f = c.xAxis[0].value;
                                        c = c.yAxis[0].value; return this.chart.addAnnotation(a({ langKey: "rectangle", type: "basicAnnotation", shapes: [{ type: "path", points: [{ xAxis: 0, yAxis: 0, x: f, y: c }, { xAxis: 0, yAxis: 0, x: f, y: c }, { xAxis: 0, yAxis: 0, x: f, y: c }, { xAxis: 0, yAxis: 0, x: f, y: c }] }] }, b.annotationsOptions, b.bindings.rectangleAnnotation.annotationsOptions))
                                    }, steps: [function (a, b) { var c = b.options.shapes[0].points, f = this.chart.pointer.getCoordinates(a); a = f.xAxis[0].value; f = f.yAxis[0].value; c[1].x = a; c[2].x = a; c[2].y = f; c[3].y = f; b.update({ shapes: [{ points: c }] }) }]
                                },
                                labelAnnotation: { className: "highcharts-label-annotation", start: function (b) { b = this.chart.pointer.getCoordinates(b); var c = this.chart.options.navigation; return this.chart.addAnnotation(a({ langKey: "label", type: "basicAnnotation", labelOptions: { format: "{y:.2f}" }, labels: [{ point: { xAxis: 0, yAxis: 0, x: b.xAxis[0].value, y: b.yAxis[0].value }, overflow: "none", crop: !0 }] }, c.annotationsOptions, c.bindings.labelAnnotation.annotationsOptions)) } }
                            }, events: {}, annotationsOptions: {}
                        }
                    }); return t
        }); p(d, "modules/stock-tools-bindings.js",
            [d["parts/Globals.js"], d["annotations/navigationBindings.js"], d["parts/Utilities.js"]], function (d, e, l) {
                var k = l.correctFloat, n = l.defined, c = l.extend, m = l.fireEvent, p = l.isNumber, b = l.merge, h = l.pick, q = l.setOptions, u = l.uniqueKey, g = e.prototype.utils; g.addFlagFromForm = function (a) {
                    return function (b) {
                        var c = this, f = c.chart, d = f.stockTools, e = g.getFieldType; b = g.attractToPoint(b, f); var h = {
                            type: "flags", onSeries: b.series.id, shape: a, data: [{ x: b.x, y: b.y }], point: {
                                events: {
                                    click: function () {
                                        var a = this, b = a.options; m(c, "showPopup",
                                            { point: a, formType: "annotation-toolbar", options: { langKey: "flags", type: "flags", title: [b.title, e(b.title)], name: [b.name, e(b.name)] }, onSubmit: function (b) { "remove" === b.actionType ? a.remove() : a.update(c.fieldsToOptions(b.fields, {})) } })
                                    }
                                }
                            }
                        }; d && d.guiEnabled || f.addSeries(h); m(c, "showPopup", { formType: "flag", options: { langKey: "flags", type: "flags", title: ["A", e("A")], name: ["Flag A", e("Flag A")] }, onSubmit: function (a) { c.fieldsToOptions(a.fields, h.data[0]); f.addSeries(h) } })
                    }
                }; g.manageIndicators = function (a) {
                    var b = this.chart,
                    c = { linkedTo: a.linkedTo, type: a.type }, d = ["ad", "cmf", "mfi", "vbp", "vwap"], e = "ad atr cci cmf macd mfi roc rsi ao aroon aroonoscillator trix apo dpo ppo natr williamsr stochastic slowstochastic linearRegression linearRegressionSlope linearRegressionIntercept linearRegressionAngle".split(" "); if ("edit" === a.actionType) this.fieldsToOptions(a.fields, c), (a = b.get(a.seriesId)) && a.update(c, !1); else if ("remove" === a.actionType) {
                        if (a = b.get(a.seriesId)) {
                            var g = a.yAxis; a.linkedSeries && a.linkedSeries.forEach(function (a) { a.remove(!1) });
                            a.remove(!1); 0 <= e.indexOf(a.type) && (g.remove(!1), this.resizeYAxes())
                        }
                    } else c.id = u(), this.fieldsToOptions(a.fields, c), 0 <= e.indexOf(a.type) ? (g = b.addAxis({ id: u(), offset: 0, opposite: !0, title: { text: "" }, tickPixelInterval: 40, showLastLabel: !1, labels: { align: "left", y: -2 } }, !1, !1), c.yAxis = g.options.id, this.resizeYAxes()) : c.yAxis = b.get(a.linkedTo).options.yAxis, 0 <= d.indexOf(a.type) && (c.params.volumeSeriesID = b.series.filter(function (a) { return "column" === a.options.type })[0].options.id), b.addSeries(c, !1); m(this, "deselectButton",
                        { button: this.selectedButtonElement }); b.redraw()
                }; g.updateHeight = function (a, b) { b.update({ typeOptions: { height: this.chart.pointer.getCoordinates(a).yAxis[0].value - b.options.typeOptions.points[1].y } }) }; g.attractToPoint = function (a, b) {
                    a = b.pointer.getCoordinates(a); var c = a.xAxis[0].value; a = a.yAxis[0].value; var f = Number.MAX_VALUE, d; b.series.forEach(function (a) { a.points.forEach(function (a) { a && f > Math.abs(a.x - c) && (f = Math.abs(a.x - c), d = a) }) }); return {
                        x: d.x, y: d.y, below: a < d.y, series: d.series, xAxis: d.series.xAxis.index ||
                            0, yAxis: d.series.yAxis.index || 0
                    }
                }; g.isNotNavigatorYAxis = function (a) { return "highcharts-navigator-yaxis" !== a.userOptions.className }; g.updateNthPoint = function (a) { return function (b, c) { var f = c.options.typeOptions; b = this.chart.pointer.getCoordinates(b); var d = b.xAxis[0].value, e = b.yAxis[0].value; f.points.forEach(function (b, c) { c >= a && (b.x = d, b.y = e) }); c.update({ typeOptions: { points: f.points } }) } }; c(e.prototype, {
                    getYAxisPositions: function (a, b, c) {
                        function f(a) { return n(a) && !p(a) && a.match("%") } var d = 0; a = a.map(function (a) {
                            var e =
                                f(a.options.height) ? parseFloat(a.options.height) / 100 : a.height / b; a = f(a.options.top) ? parseFloat(a.options.top) / 100 : k(a.top - a.chart.plotTop) / b; p(e) || (e = c / 100); d = k(d + e); return { height: 100 * e, top: 100 * a }
                        }); a.allAxesHeight = d; return a
                    }, getYAxisResizers: function (a) { var b = []; a.forEach(function (c, d) { c = a[d + 1]; b[d] = c ? { enabled: !0, controlledAxis: { next: [h(c.options.id, c.options.index)] } } : { enabled: !1 } }); return b }, resizeYAxes: function (a) {
                        a = a || 20; var b = this.chart, c = b.yAxis.filter(g.isNotNavigatorYAxis), d = c.length; b = this.getYAxisPositions(c,
                            b.plotHeight, a); var e = this.getYAxisResizers(c), h = b.allAxesHeight, l = a; 1 < h ? (6 > d ? (b[0].height = k(b[0].height - l), b = this.recalculateYAxisPositions(b, l)) : (a = 100 / d, b = this.recalculateYAxisPositions(b, a / (d - 1), !0, -1)), b[d - 1] = { top: k(100 - a), height: a }) : (l = 100 * k(1 - h), 5 > d ? (b[0].height = k(b[0].height + l), b = this.recalculateYAxisPositions(b, l)) : b = this.recalculateYAxisPositions(b, l / d, !0, 1)); b.forEach(function (a, b) { c[b].update({ height: a.height + "%", top: a.top + "%", resize: e[b] }, !1) })
                    }, recalculateYAxisPositions: function (a, b,
                        c, d) { a.forEach(function (f, e) { e = a[e - 1]; f.top = e ? k(e.height + e.top) : 0; c && (f.height = k(f.height + d * b)) }); return a }
                }); l = {
                    segment: { className: "highcharts-segment", start: function (a) { a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({ langKey: "segment", type: "crookedLine", typeOptions: { points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }] } }, c.annotationsOptions, c.bindings.segment.annotationsOptions); return this.chart.addAnnotation(a) }, steps: [g.updateNthPoint(1)] },
                    arrowSegment: { className: "highcharts-arrow-segment", start: function (a) { a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({ langKey: "arrowSegment", type: "crookedLine", typeOptions: { line: { markerEnd: "arrow" }, points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }] } }, c.annotationsOptions, c.bindings.arrowSegment.annotationsOptions); return this.chart.addAnnotation(a) }, steps: [g.updateNthPoint(1)] }, ray: {
                        className: "highcharts-ray", start: function (a) {
                            a =
                            this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({ langKey: "ray", type: "crookedLine", typeOptions: { type: "ray", points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }] } }, c.annotationsOptions, c.bindings.ray.annotationsOptions); return this.chart.addAnnotation(a)
                        }, steps: [g.updateNthPoint(1)]
                    }, arrowRay: {
                        className: "highcharts-arrow-ray", start: function (a) {
                            a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({
                                langKey: "arrowRay",
                                type: "infinityLine", typeOptions: { type: "ray", line: { markerEnd: "arrow" }, points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }] }
                            }, c.annotationsOptions, c.bindings.arrowRay.annotationsOptions); return this.chart.addAnnotation(a)
                        }, steps: [g.updateNthPoint(1)]
                    }, infinityLine: {
                        className: "highcharts-infinity-line", start: function (a) {
                            a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({
                                langKey: "infinityLine", type: "infinityLine", typeOptions: {
                                    type: "line",
                                    points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }]
                                }
                            }, c.annotationsOptions, c.bindings.infinityLine.annotationsOptions); return this.chart.addAnnotation(a)
                        }, steps: [g.updateNthPoint(1)]
                    }, arrowInfinityLine: {
                        className: "highcharts-arrow-infinity-line", start: function (a) {
                            a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({
                                langKey: "arrowInfinityLine", type: "infinityLine", typeOptions: {
                                    type: "line", line: { markerEnd: "arrow" }, points: [{
                                        x: a.xAxis[0].value,
                                        y: a.yAxis[0].value
                                    }, { x: a.xAxis[0].value, y: a.yAxis[0].value }]
                                }
                            }, c.annotationsOptions, c.bindings.arrowInfinityLine.annotationsOptions); return this.chart.addAnnotation(a)
                        }, steps: [g.updateNthPoint(1)]
                    }, horizontalLine: {
                        className: "highcharts-horizontal-line", start: function (a) {
                            a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({ langKey: "horizontalLine", type: "infinityLine", draggable: "y", typeOptions: { type: "horizontalLine", points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }] } }, c.annotationsOptions,
                                c.bindings.horizontalLine.annotationsOptions); this.chart.addAnnotation(a)
                        }
                    }, verticalLine: { className: "highcharts-vertical-line", start: function (a) { a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({ langKey: "verticalLine", type: "infinityLine", draggable: "x", typeOptions: { type: "verticalLine", points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }] } }, c.annotationsOptions, c.bindings.verticalLine.annotationsOptions); this.chart.addAnnotation(a) } }, crooked3: {
                        className: "highcharts-crooked3",
                        start: function (a) { a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({ langKey: "crooked3", type: "crookedLine", typeOptions: { points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }] } }, c.annotationsOptions, c.bindings.crooked3.annotationsOptions); return this.chart.addAnnotation(a) }, steps: [g.updateNthPoint(1), g.updateNthPoint(2)]
                    }, crooked5: {
                        className: "highcharts-crooked5", start: function (a) {
                            a = this.chart.pointer.getCoordinates(a);
                            var c = this.chart.options.navigation; a = b({ langKey: "crookedLine", type: "crookedLine", typeOptions: { points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }] } }, c.annotationsOptions, c.bindings.crooked5.annotationsOptions); return this.chart.addAnnotation(a)
                        }, steps: [g.updateNthPoint(1), g.updateNthPoint(2), g.updateNthPoint(3), g.updateNthPoint(4)]
                    }, elliott3: {
                        className: "highcharts-elliott3",
                        start: function (a) { a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({ langKey: "elliott3", type: "elliottWave", typeOptions: { points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }] }, labelOptions: { style: { color: "#666666" } } }, c.annotationsOptions, c.bindings.elliott3.annotationsOptions); return this.chart.addAnnotation(a) }, steps: [g.updateNthPoint(1), g.updateNthPoint(2),
                        g.updateNthPoint(3)]
                    }, elliott5: {
                        className: "highcharts-elliott5", start: function (a) {
                            a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({ langKey: "elliott5", type: "elliottWave", typeOptions: { points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }] }, labelOptions: { style: { color: "#666666" } } },
                                c.annotationsOptions, c.bindings.elliott5.annotationsOptions); return this.chart.addAnnotation(a)
                        }, steps: [g.updateNthPoint(1), g.updateNthPoint(2), g.updateNthPoint(3), g.updateNthPoint(4), g.updateNthPoint(5)]
                    }, measureX: {
                        className: "highcharts-measure-x", start: function (a) {
                            a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({
                                langKey: "measure", type: "measure", typeOptions: {
                                    selectType: "x", point: { x: a.xAxis[0].value, y: a.yAxis[0].value, xAxis: 0, yAxis: 0 }, crosshairX: { strokeWidth: 1, stroke: "#000000" },
                                    crosshairY: { enabled: !1, strokeWidth: 0, stroke: "#000000" }, background: { width: 0, height: 0, strokeWidth: 0, stroke: "#ffffff" }
                                }, labelOptions: { style: { color: "#666666" } }
                            }, c.annotationsOptions, c.bindings.measureX.annotationsOptions); return this.chart.addAnnotation(a)
                        }, steps: [g.updateRectSize]
                    }, measureY: {
                        className: "highcharts-measure-y", start: function (a) {
                            a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({
                                langKey: "measure", type: "measure", typeOptions: {
                                    selectType: "y", point: {
                                        x: a.xAxis[0].value,
                                        y: a.yAxis[0].value, xAxis: 0, yAxis: 0
                                    }, crosshairX: { enabled: !1, strokeWidth: 0, stroke: "#000000" }, crosshairY: { strokeWidth: 1, stroke: "#000000" }, background: { width: 0, height: 0, strokeWidth: 0, stroke: "#ffffff" }
                                }, labelOptions: { style: { color: "#666666" } }
                            }, c.annotationsOptions, c.bindings.measureY.annotationsOptions); return this.chart.addAnnotation(a)
                        }, steps: [g.updateRectSize]
                    }, measureXY: {
                        className: "highcharts-measure-xy", start: function (a) {
                            a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({
                                langKey: "measure",
                                type: "measure", typeOptions: { selectType: "xy", point: { x: a.xAxis[0].value, y: a.yAxis[0].value, xAxis: 0, yAxis: 0 }, background: { width: 0, height: 0, strokeWidth: 10 }, crosshairX: { strokeWidth: 1, stroke: "#000000" }, crosshairY: { strokeWidth: 1, stroke: "#000000" } }, labelOptions: { style: { color: "#666666" } }
                            }, c.annotationsOptions, c.bindings.measureXY.annotationsOptions); return this.chart.addAnnotation(a)
                        }, steps: [g.updateRectSize]
                    }, fibonacci: {
                        className: "highcharts-fibonacci", start: function (a) {
                            a = this.chart.pointer.getCoordinates(a);
                            var c = this.chart.options.navigation; a = b({ langKey: "fibonacci", type: "fibonacci", typeOptions: { points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }] }, labelOptions: { style: { color: "#666666" } } }, c.annotationsOptions, c.bindings.fibonacci.annotationsOptions); return this.chart.addAnnotation(a)
                        }, steps: [g.updateNthPoint(1), g.updateHeight]
                    }, parallelChannel: {
                        className: "highcharts-parallel-channel", start: function (a) {
                            a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation;
                            a = b({ langKey: "parallelChannel", type: "tunnel", typeOptions: { points: [{ x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }] } }, c.annotationsOptions, c.bindings.parallelChannel.annotationsOptions); return this.chart.addAnnotation(a)
                        }, steps: [g.updateNthPoint(1), g.updateHeight]
                    }, pitchfork: {
                        className: "highcharts-pitchfork", start: function (a) {
                            a = this.chart.pointer.getCoordinates(a); var c = this.chart.options.navigation; a = b({
                                langKey: "pitchfork", type: "pitchfork", typeOptions: {
                                    points: [{
                                        x: a.xAxis[0].value,
                                        y: a.yAxis[0].value, controlPoint: { style: { fill: "red" } }
                                    }, { x: a.xAxis[0].value, y: a.yAxis[0].value }, { x: a.xAxis[0].value, y: a.yAxis[0].value }], innerBackground: { fill: "rgba(100, 170, 255, 0.8)" }
                                }, shapeOptions: { strokeWidth: 2 }
                            }, c.annotationsOptions, c.bindings.pitchfork.annotationsOptions); return this.chart.addAnnotation(a)
                        }, steps: [g.updateNthPoint(1), g.updateNthPoint(2)]
                    }, verticalCounter: {
                        className: "highcharts-vertical-counter", start: function (a) {
                            a = g.attractToPoint(a, this.chart); var c = this.chart.options.navigation,
                                d = n(this.verticalCounter) ? this.verticalCounter : 0; a = b({ langKey: "verticalCounter", type: "verticalLine", typeOptions: { point: { x: a.x, y: a.y, xAxis: a.xAxis, yAxis: a.yAxis }, label: { offset: a.below ? 40 : -40, text: d.toString() } }, labelOptions: { style: { color: "#666666", fontSize: "11px" } }, shapeOptions: { stroke: "rgba(0, 0, 0, 0.75)", strokeWidth: 1 } }, c.annotationsOptions, c.bindings.verticalCounter.annotationsOptions); a = this.chart.addAnnotation(a); a.options.events.click.call(a, {})
                        }
                    }, verticalLabel: {
                        className: "highcharts-vertical-label",
                        start: function (a) { a = g.attractToPoint(a, this.chart); var c = this.chart.options.navigation; a = b({ langKey: "verticalLabel", type: "verticalLine", typeOptions: { point: { x: a.x, y: a.y, xAxis: a.xAxis, yAxis: a.yAxis }, label: { offset: a.below ? 40 : -40 } }, labelOptions: { style: { color: "#666666", fontSize: "11px" } }, shapeOptions: { stroke: "rgba(0, 0, 0, 0.75)", strokeWidth: 1 } }, c.annotationsOptions, c.bindings.verticalLabel.annotationsOptions); a = this.chart.addAnnotation(a); a.options.events.click.call(a, {}) }
                    }, verticalArrow: {
                        className: "highcharts-vertical-arrow",
                        start: function (a) { a = g.attractToPoint(a, this.chart); var c = this.chart.options.navigation; a = b({ langKey: "verticalArrow", type: "verticalLine", typeOptions: { point: { x: a.x, y: a.y, xAxis: a.xAxis, yAxis: a.yAxis }, label: { offset: a.below ? 40 : -40, format: " " }, connector: { fill: "none", stroke: a.below ? "red" : "green" } }, shapeOptions: { stroke: "rgba(0, 0, 0, 0.75)", strokeWidth: 1 } }, c.annotationsOptions, c.bindings.verticalArrow.annotationsOptions); a = this.chart.addAnnotation(a); a.options.events.click.call(a, {}) }
                    }, flagCirclepin: {
                        className: "highcharts-flag-circlepin",
                        start: g.addFlagFromForm("circlepin")
                    }, flagDiamondpin: { className: "highcharts-flag-diamondpin", start: g.addFlagFromForm("flag") }, flagSquarepin: { className: "highcharts-flag-squarepin", start: g.addFlagFromForm("squarepin") }, flagSimplepin: { className: "highcharts-flag-simplepin", start: g.addFlagFromForm("nopin") }, zoomX: { className: "highcharts-zoom-x", init: function (a) { this.chart.update({ chart: { zoomType: "x" } }); m(this, "deselectButton", { button: a }) } }, zoomY: {
                        className: "highcharts-zoom-y", init: function (a) {
                            this.chart.update({ chart: { zoomType: "y" } });
                            m(this, "deselectButton", { button: a })
                        }
                    }, zoomXY: { className: "highcharts-zoom-xy", init: function (a) { this.chart.update({ chart: { zoomType: "xy" } }); m(this, "deselectButton", { button: a }) } }, seriesTypeLine: { className: "highcharts-series-type-line", init: function (a) { this.chart.series[0].update({ type: "line", useOhlcData: !0 }); m(this, "deselectButton", { button: a }) } }, seriesTypeOhlc: { className: "highcharts-series-type-ohlc", init: function (a) { this.chart.series[0].update({ type: "ohlc" }); m(this, "deselectButton", { button: a }) } }, seriesTypeCandlestick: {
                        className: "highcharts-series-type-candlestick",
                        init: function (a) { this.chart.series[0].update({ type: "candlestick" }); m(this, "deselectButton", { button: a }) }
                    }, fullScreen: { className: "highcharts-full-screen", init: function (a) { this.chart.fullscreen.toggle(); m(this, "deselectButton", { button: a }) } }, currentPriceIndicator: {
                        className: "highcharts-current-price-indicator", init: function (a) {
                            var c = this.chart, b = c.series[0], d = b.options, e = d.lastVisiblePrice && d.lastVisiblePrice.enabled; d = d.lastPrice && d.lastPrice.enabled; c = c.stockTools; var g = c.getIconsURL(); c && c.guiEnabled &&
                                (a.firstChild.style["background-image"] = d ? 'url("' + g + 'current-price-show.svg")' : 'url("' + g + 'current-price-hide.svg")'); b.update({ lastPrice: { enabled: !d, color: "red" }, lastVisiblePrice: { enabled: !e, label: { enabled: !0 } } }); m(this, "deselectButton", { button: a })
                        }
                    }, indicators: { className: "highcharts-indicators", init: function () { var a = this; m(a, "showPopup", { formType: "indicators", options: {}, onSubmit: function (c) { a.utils.manageIndicators.call(a, c) } }) } }, toggleAnnotations: {
                        className: "highcharts-toggle-annotations", init: function (a) {
                            var c =
                                this.chart, b = c.stockTools, d = b.getIconsURL(); this.toggledAnnotations = !this.toggledAnnotations; (c.annotations || []).forEach(function (a) { a.setVisibility(!this.toggledAnnotations) }, this); b && b.guiEnabled && (a.firstChild.style["background-image"] = this.toggledAnnotations ? 'url("' + d + 'annotations-hidden.svg")' : 'url("' + d + 'annotations-visible.svg")'); m(this, "deselectButton", { button: a })
                        }
                    }, saveChart: {
                        className: "highcharts-save-chart", init: function (a) {
                            var c = this.chart, b = [], e = [], h = [], k = []; c.annotations.forEach(function (a,
                                c) { b[c] = a.userOptions }); c.series.forEach(function (a) { a.is("sma") ? e.push(a.userOptions) : "flags" === a.type && h.push(a.userOptions) }); c.yAxis.forEach(function (a) { g.isNotNavigatorYAxis(a) && k.push(a.options) }); d.win.localStorage.setItem("highcharts-chart", JSON.stringify({ annotations: b, indicators: e, flags: h, yAxes: k })); m(this, "deselectButton", { button: a })
                        }
                    }
                }; q({ navigation: { bindings: l } }); e.prototype.utils = b(g, e.prototype.utils)
            }); p(d, "modules/stock-tools-gui.js", [d["parts/Chart.js"], d["parts/Globals.js"], d["annotations/navigationBindings.js"],
            d["parts/Utilities.js"]], function (d, e, l, k) {
                var n = k.addEvent, c = k.createElement, m = k.css, p = k.extend, b = k.fireEvent, h = k.getStyle, q = k.isArray, u = k.merge, g = k.pick; k = k.setOptions; k({
                    lang: {
                        stockTools: {
                            gui: {
                                simpleShapes: "Simple shapes", lines: "Lines", crookedLines: "Crooked lines", measure: "Measure", advanced: "Advanced", toggleAnnotations: "Toggle annotations", verticalLabels: "Vertical labels", flags: "Flags", zoomChange: "Zoom change", typeChange: "Type change", saveChart: "Save chart", indicators: "Indicators", currentPriceIndicator: "Current Price Indicators",
                                zoomX: "Zoom X", zoomY: "Zoom Y", zoomXY: "Zooom XY", fullScreen: "Fullscreen", typeOHLC: "OHLC", typeLine: "Line", typeCandlestick: "Candlestick", circle: "Circle", label: "Label", rectangle: "Rectangle", flagCirclepin: "Flag circle", flagDiamondpin: "Flag diamond", flagSquarepin: "Flag square", flagSimplepin: "Flag simple", measureXY: "Measure XY", measureX: "Measure X", measureY: "Measure Y", segment: "Segment", arrowSegment: "Arrow segment", ray: "Ray", arrowRay: "Arrow ray", line: "Line", arrowLine: "Arrow line", horizontalLine: "Horizontal line",
                                verticalLine: "Vertical line", infinityLine: "Infinity line", crooked3: "Crooked 3 line", crooked5: "Crooked 5 line", elliott3: "Elliott 3 line", elliott5: "Elliott 5 line", verticalCounter: "Vertical counter", verticalLabel: "Vertical label", verticalArrow: "Vertical arrow", fibonacci: "Fibonacci", pitchfork: "Pitchfork", parallelChannel: "Parallel channel"
                            }
                        }, navigation: {
                            popup: {
                                circle: "Circle", rectangle: "Rectangle", label: "Label", segment: "Segment", arrowSegment: "Arrow segment", ray: "Ray", arrowRay: "Arrow ray", line: "Line",
                                arrowLine: "Arrow line", horizontalLine: "Horizontal line", verticalLine: "Vertical line", crooked3: "Crooked 3 line", crooked5: "Crooked 5 line", elliott3: "Elliott 3 line", elliott5: "Elliott 5 line", verticalCounter: "Vertical counter", verticalLabel: "Vertical label", verticalArrow: "Vertical arrow", fibonacci: "Fibonacci", pitchfork: "Pitchfork", parallelChannel: "Parallel channel", infinityLine: "Infinity line", measure: "Measure", measureXY: "Measure XY", measureX: "Measure X", measureY: "Measure Y", flags: "Flags", addButton: "add",
                                saveButton: "save", editButton: "edit", removeButton: "remove", series: "Series", volume: "Volume", connector: "Connector", innerBackground: "Inner background", outerBackground: "Outer background", crosshairX: "Crosshair X", crosshairY: "Crosshair Y", tunnel: "Tunnel", background: "Background"
                            }
                        }
                    }, stockTools: {
                        gui: {
                            enabled: !0, className: "highcharts-bindings-wrapper", toolbarClassName: "stocktools-toolbar", buttons: "indicators separator simpleShapes lines crookedLines measure advanced toggleAnnotations separator verticalLabels flags separator zoomChange fullScreen typeChange separator currentPriceIndicator saveChart".split(" "),
                            definitions: {
                                separator: { symbol: "separator.svg" }, simpleShapes: { items: ["label", "circle", "rectangle"], circle: { symbol: "circle.svg" }, rectangle: { symbol: "rectangle.svg" }, label: { symbol: "label.svg" } }, flags: { items: ["flagCirclepin", "flagDiamondpin", "flagSquarepin", "flagSimplepin"], flagSimplepin: { symbol: "flag-basic.svg" }, flagDiamondpin: { symbol: "flag-diamond.svg" }, flagSquarepin: { symbol: "flag-trapeze.svg" }, flagCirclepin: { symbol: "flag-elipse.svg" } }, lines: {
                                    items: "segment arrowSegment ray arrowRay line arrowLine horizontalLine verticalLine".split(" "),
                                    segment: { symbol: "segment.svg" }, arrowSegment: { symbol: "arrow-segment.svg" }, ray: { symbol: "ray.svg" }, arrowRay: { symbol: "arrow-ray.svg" }, line: { symbol: "line.svg" }, arrowLine: { symbol: "arrow-line.svg" }, verticalLine: { symbol: "vertical-line.svg" }, horizontalLine: { symbol: "horizontal-line.svg" }
                                }, crookedLines: { items: ["elliott3", "elliott5", "crooked3", "crooked5"], crooked3: { symbol: "crooked-3.svg" }, crooked5: { symbol: "crooked-5.svg" }, elliott3: { symbol: "elliott-3.svg" }, elliott5: { symbol: "elliott-5.svg" } }, verticalLabels: {
                                    items: ["verticalCounter",
                                        "verticalLabel", "verticalArrow"], verticalCounter: { symbol: "vertical-counter.svg" }, verticalLabel: { symbol: "vertical-label.svg" }, verticalArrow: { symbol: "vertical-arrow.svg" }
                                }, advanced: { items: ["fibonacci", "pitchfork", "parallelChannel"], pitchfork: { symbol: "pitchfork.svg" }, fibonacci: { symbol: "fibonacci.svg" }, parallelChannel: { symbol: "parallel-channel.svg" } }, measure: { items: ["measureXY", "measureX", "measureY"], measureX: { symbol: "measure-x.svg" }, measureY: { symbol: "measure-y.svg" }, measureXY: { symbol: "measure-xy.svg" } },
                                toggleAnnotations: { symbol: "annotations-visible.svg" }, currentPriceIndicator: { symbol: "current-price-show.svg" }, indicators: { symbol: "indicators.svg" }, zoomChange: { items: ["zoomX", "zoomY", "zoomXY"], zoomX: { symbol: "zoom-x.svg" }, zoomY: { symbol: "zoom-y.svg" }, zoomXY: { symbol: "zoom-xy.svg" } }, typeChange: { items: ["typeOHLC", "typeLine", "typeCandlestick"], typeOHLC: { symbol: "series-ohlc.svg" }, typeLine: { symbol: "series-line.svg" }, typeCandlestick: { symbol: "series-candlestick.svg" } }, fullScreen: { symbol: "fullscreen.svg" }, saveChart: { symbol: "save-chart.svg" }
                            }
                        }
                    }
                });
                n(e.Chart, "afterGetContainer", function () { this.setStockTools() }); n(e.Chart, "getMargins", function () { var a = this.stockTools && this.stockTools.listWrapper; (a = a && (a.startWidth + h(a, "padding-left") + h(a, "padding-right") || a.offsetWidth)) && a < this.plotWidth && (this.plotLeft += a) }); n(e.Chart, "destroy", function () { this.stockTools && this.stockTools.destroy() }); n(e.Chart, "redraw", function () { this.stockTools && this.stockTools.guiEnabled && this.stockTools.redraw() }); k = function () {
                    function a(a, c, d) {
                        this.wrapper = this.toolbar =
                            this.submenu = this.showhideBtn = this.listWrapper = this.arrowWrapper = this.arrowUp = this.arrowDown = void 0; this.chart = d; this.options = a; this.lang = c; this.iconsURL = this.getIconsURL(); this.guiEnabled = a.enabled; this.visible = g(a.visible, !0); this.placed = g(a.placed, !1); this.eventsToUnbind = []; this.guiEnabled && (this.createHTML(), this.init(), this.showHideNavigatorion()); b(this, "afterInit")
                    } a.prototype.init = function () {
                        var a = this, c = this.lang, b = this.options, d = this.toolbar, e = a.addSubmenu, g = b.definitions, h = d.childNodes,
                        k; b.buttons.forEach(function (b) { k = a.addButton(d, g, b, c); a.eventsToUnbind.push(n(k.buttonWrapper, "click", function () { a.eraseActiveButtons(h, k.buttonWrapper) })); q(g[b].items) && e.call(a, k, g[b]) })
                    }; a.prototype.addSubmenu = function (a, b) {
                        var d = this, e = a.submenuArrow, f = a.buttonWrapper, g = h(f, "width"), k = this.wrapper, l = this.listWrapper, v = this.toolbar.childNodes, p = 0, q; this.submenu = q = c("ul", { className: "highcharts-submenu-wrapper" }, null, f); this.addSubmenuItems(f, b); d.eventsToUnbind.push(n(e, "click", function (a) {
                            a.stopPropagation();
                            d.eraseActiveButtons(v, f); 0 <= f.className.indexOf("highcharts-current") ? (l.style.width = l.startWidth + "px", f.classList.remove("highcharts-current"), q.style.display = "none") : (q.style.display = "block", p = q.offsetHeight - f.offsetHeight - 3, q.offsetHeight + f.offsetTop > k.offsetHeight && f.offsetTop > p || (p = 0), m(q, { top: -p + "px", left: g + 3 + "px" }), f.className += " highcharts-current", l.startWidth = k.offsetWidth, l.style.width = l.startWidth + h(l, "padding-left") + q.offsetWidth + 3 + "px")
                        }))
                    }; a.prototype.addSubmenuItems = function (a, c) {
                        var b =
                            this, d = this.submenu, e = this.lang, f = this.listWrapper, g; c.items.forEach(function (h) { g = b.addButton(d, c, h, e); b.eventsToUnbind.push(n(g.mainButton, "click", function () { b.switchSymbol(this, a, !0); f.style.width = f.startWidth + "px"; d.style.display = "none" })) }); var h = d.querySelectorAll("li > .highcharts-menu-item-btn")[0]; b.switchSymbol(h, !1)
                    }; a.prototype.eraseActiveButtons = function (a, c, b) {
                        [].forEach.call(a, function (a) {
                            a !== c && (a.classList.remove("highcharts-current"), a.classList.remove("highcharts-active"), b = a.querySelectorAll(".highcharts-submenu-wrapper"),
                                0 < b.length && (b[0].style.display = "none"))
                        })
                    }; a.prototype.addButton = function (b, d, e, h) {
                        void 0 === h && (h = {}); d = d[e]; var f = d.items, k = d.className || ""; e = c("li", { className: g(a.prototype.classMapping[e], "") + " " + k, title: h[e] || e }, null, b); b = c("span", { className: "highcharts-menu-item-btn" }, null, e); if (f && f.length) { var l = c("span", { className: "highcharts-submenu-item-arrow highcharts-arrow-right" }, null, e); l.style["background-image"] = "url(" + this.iconsURL + "arrow-bottom.svg)" } else b.style["background-image"] = "url(" + this.iconsURL +
                            d.symbol + ")"; return { buttonWrapper: e, mainButton: b, submenuArrow: l }
                    }; a.prototype.addNavigation = function () {
                        var a = this.wrapper; this.arrowWrapper = c("div", { className: "highcharts-arrow-wrapper" }); this.arrowUp = c("div", { className: "highcharts-arrow-up" }, null, this.arrowWrapper); this.arrowUp.style["background-image"] = "url(" + this.iconsURL + "arrow-right.svg)"; this.arrowDown = c("div", { className: "highcharts-arrow-down" }, null, this.arrowWrapper); this.arrowDown.style["background-image"] = "url(" + this.iconsURL + "arrow-right.svg)";
                        a.insertBefore(this.arrowWrapper, a.childNodes[0]); this.scrollButtons()
                    }; a.prototype.scrollButtons = function () { var a = 0, c = this.wrapper, b = this.toolbar, d = .1 * c.offsetHeight; this.eventsToUnbind.push(n(this.arrowUp, "click", function () { 0 < a && (a -= d, b.style["margin-top"] = -a + "px") })); this.eventsToUnbind.push(n(this.arrowDown, "click", function () { c.offsetHeight + a <= b.offsetHeight + d && (a += d, b.style["margin-top"] = -a + "px") })) }; a.prototype.createHTML = function () {
                        var a = this.chart, b = this.options, d = a.container; a = a.options.navigation;
                        this.wrapper = a = c("div", { className: "highcharts-stocktools-wrapper " + b.className + " " + (a && a.bindingsClassName) }); d.parentNode.insertBefore(a, d); this.toolbar = d = c("ul", { className: "highcharts-stocktools-toolbar " + b.toolbarClassName }); this.listWrapper = b = c("div", { className: "highcharts-menu-wrapper" }); a.insertBefore(b, a.childNodes[0]); b.insertBefore(d, b.childNodes[0]); this.showHideToolbar(); this.addNavigation()
                    }; a.prototype.showHideNavigatorion = function () {
                        this.visible && this.toolbar.offsetHeight > this.wrapper.offsetHeight -
                            50 ? this.arrowWrapper.style.display = "block" : (this.toolbar.style.marginTop = "0px", this.arrowWrapper.style.display = "none")
                    }; a.prototype.showHideToolbar = function () {
                        var a = this.chart, b = this.wrapper, d = this.listWrapper, e = this.submenu, g = this.visible, k; this.showhideBtn = k = c("div", { className: "highcharts-toggle-toolbar highcharts-arrow-left" }, null, b); k.style["background-image"] = "url(" + this.iconsURL + "arrow-right.svg)"; g ? (b.style.height = "100%", k.style.top = h(d, "padding-top") + "px", k.style.left = b.offsetWidth + h(d, "padding-left") +
                            "px") : (e && (e.style.display = "none"), k.style.left = "0px", this.visible = g = !1, d.classList.add("highcharts-hide"), k.classList.toggle("highcharts-arrow-right"), b.style.height = k.offsetHeight + "px"); this.eventsToUnbind.push(n(k, "click", function () { a.update({ stockTools: { gui: { visible: !g, placed: !0 } } }) }))
                    }; a.prototype.switchSymbol = function (a, b) {
                        var c = a.parentNode, d = c.classList.value; c = c.parentNode.parentNode; c.className = ""; d && c.classList.add(d.trim()); c.querySelectorAll(".highcharts-menu-item-btn")[0].style["background-image"] =
                            a.style["background-image"]; b && this.selectButton(c)
                    }; a.prototype.selectButton = function (a) { 0 <= a.className.indexOf("highcharts-active") ? a.classList.remove("highcharts-active") : a.classList.add("highcharts-active") }; a.prototype.unselectAllButtons = function (a) { var b = a.parentNode.querySelectorAll(".highcharts-active");[].forEach.call(b, function (b) { b !== a && b.classList.remove("highcharts-active") }) }; a.prototype.update = function (a) {
                        u(!0, this.chart.options.stockTools, a); this.destroy(); this.chart.setStockTools(a);
                        this.chart.navigationBindings && this.chart.navigationBindings.update()
                    }; a.prototype.destroy = function () { var a = this.wrapper, b = a && a.parentNode; this.eventsToUnbind.forEach(function (a) { a() }); b && b.removeChild(a); this.chart.isDirtyBox = !0; this.chart.redraw() }; a.prototype.redraw = function () { this.showHideNavigatorion() }; a.prototype.getIconsURL = function () { return this.chart.options.navigation.iconsURL || this.options.iconsURL || "https://code.highcharts.com/8.1.2/gfx/stock-icons/" }; return a
                }(); k.prototype.classMapping =
                {
                    circle: "highcharts-circle-annotation", rectangle: "highcharts-rectangle-annotation", label: "highcharts-label-annotation", segment: "highcharts-segment", arrowSegment: "highcharts-arrow-segment", ray: "highcharts-ray", arrowRay: "highcharts-arrow-ray", line: "highcharts-infinity-line", arrowLine: "highcharts-arrow-infinity-line", verticalLine: "highcharts-vertical-line", horizontalLine: "highcharts-horizontal-line", crooked3: "highcharts-crooked3", crooked5: "highcharts-crooked5", elliott3: "highcharts-elliott3", elliott5: "highcharts-elliott5",
                    pitchfork: "highcharts-pitchfork", fibonacci: "highcharts-fibonacci", parallelChannel: "highcharts-parallel-channel", measureX: "highcharts-measure-x", measureY: "highcharts-measure-y", measureXY: "highcharts-measure-xy", verticalCounter: "highcharts-vertical-counter", verticalLabel: "highcharts-vertical-label", verticalArrow: "highcharts-vertical-arrow", currentPriceIndicator: "highcharts-current-price-indicator", indicators: "highcharts-indicators", flagCirclepin: "highcharts-flag-circlepin", flagDiamondpin: "highcharts-flag-diamondpin",
                    flagSquarepin: "highcharts-flag-squarepin", flagSimplepin: "highcharts-flag-simplepin", zoomX: "highcharts-zoom-x", zoomY: "highcharts-zoom-y", zoomXY: "highcharts-zoom-xy", typeLine: "highcharts-series-type-line", typeOHLC: "highcharts-series-type-ohlc", typeCandlestick: "highcharts-series-type-candlestick", fullScreen: "highcharts-full-screen", toggleAnnotations: "highcharts-toggle-annotations", saveChart: "highcharts-save-chart", separator: "highcharts-separator"
                }; p(d.prototype, {
                    setStockTools: function (a) {
                        var b = this.options,
                        c = b.lang; a = u(b.stockTools && b.stockTools.gui, a && a.gui); this.stockTools = new e.Toolbar(a, c.stockTools && c.stockTools.gui, this); this.stockTools.guiEnabled && (this.isDirtyBox = !0)
                    }
                }); n(l, "selectButton", function (a) { var b = a.button, c = this.chart.stockTools; c && c.guiEnabled && (c.unselectAllButtons(a.button), 0 <= b.parentNode.className.indexOf("highcharts-submenu-wrapper") && (b = b.parentNode.parentNode), c.selectButton(b)) }); n(l, "deselectButton", function (a) {
                    a = a.button; var b = this.chart.stockTools; b && b.guiEnabled && (0 <=
                        a.parentNode.className.indexOf("highcharts-submenu-wrapper") && (a = a.parentNode.parentNode), b.selectButton(a))
                }); e.Toolbar = k; return e.Toolbar
            }); p(d, "masters/modules/stock-tools.src.js", [], function () { })
});
//# sourceMappingURL=stock-tools.js.map