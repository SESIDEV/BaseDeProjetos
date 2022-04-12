/*
 Highcharts JS v8.1.2 (2020-06-16)

 Force directed graph module

 (c) 2010-2019 Torstein Honsi

 License: www.highcharts.com/license
*/
(function (f) { "object" === typeof module && module.exports ? (f["default"] = f, module.exports = f) : "function" === typeof define && define.amd ? define("highcharts/modules/networkgraph", ["highcharts"], function (m) { f(m); f.Highcharts = m; return f }) : f("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (f) {
    function m(f, b, a, e) { f.hasOwnProperty(b) || (f[b] = e.apply(null, a)) } f = f ? f._modules : {}; m(f, "mixins/nodes.js", [f["parts/Globals.js"], f["parts/Point.js"], f["parts/Utilities.js"]], function (f, b, a) {
        var e = a.defined, d = a.extend,
        g = a.find, n = a.pick; f.NodesMixin = {
            createNode: function (b) {
                function a(c, h) { return g(c, function (c) { return c.id === h }) } var e = a(this.nodes, b), c = this.pointClass; if (!e) {
                    var h = this.options.nodes && a(this.options.nodes, b); e = (new c).init(this, d({ className: "highcharts-node", isNode: !0, id: b, y: 1 }, h)); e.linksTo = []; e.linksFrom = []; e.formatPrefix = "node"; e.name = e.name || e.options.id || ""; e.mass = n(e.options.mass, e.options.marker && e.options.marker.radius, this.options.marker && this.options.marker.radius, 4); e.getSum = function () {
                        var c =
                            0, h = 0; e.linksTo.forEach(function (h) { c += h.weight }); e.linksFrom.forEach(function (c) { h += c.weight }); return Math.max(c, h)
                    }; e.offset = function (c, h) { for (var b = 0, d = 0; d < e[h].length; d++) { if (e[h][d] === c) return b; b += e[h][d].weight } }; e.hasShape = function () { var c = 0; e.linksTo.forEach(function (h) { h.outgoing && c++ }); return !e.linksTo.length || c !== e.linksTo.length }; this.nodes.push(e)
                } return e
            }, generatePoints: function () {
                var b = this.chart, d = {}; f.Series.prototype.generatePoints.call(this); this.nodes || (this.nodes = []); this.colorCounter =
                    0; this.nodes.forEach(function (d) { d.linksFrom.length = 0; d.linksTo.length = 0; d.level = d.options.level }); this.points.forEach(function (a) { e(a.from) && (d[a.from] || (d[a.from] = this.createNode(a.from)), d[a.from].linksFrom.push(a), a.fromNode = d[a.from], b.styledMode ? a.colorIndex = n(a.options.colorIndex, d[a.from].colorIndex) : a.color = a.options.color || d[a.from].color); e(a.to) && (d[a.to] || (d[a.to] = this.createNode(a.to)), d[a.to].linksTo.push(a), a.toNode = d[a.to]); a.name = a.name || a.id }, this); this.nodeLookup = d
            }, setData: function () {
                this.nodes &&
                (this.nodes.forEach(function (a) { a.destroy() }), this.nodes.length = 0); f.Series.prototype.setData.apply(this, arguments)
            }, destroy: function () { this.data = [].concat(this.points || [], this.nodes); return f.Series.prototype.destroy.apply(this, arguments) }, setNodeState: function (a) {
                var d = arguments, e = this.isNode ? this.linksTo.concat(this.linksFrom) : [this.fromNode, this.toNode]; "select" !== a && e.forEach(function (c) {
                    c && c.series && (b.prototype.setState.apply(c, d), c.isNode || (c.fromNode.graphic && b.prototype.setState.apply(c.fromNode,
                        d), c.toNode && c.toNode.graphic && b.prototype.setState.apply(c.toNode, d)))
                }); b.prototype.setState.apply(this, d)
            }
        }
    }); m(f, "modules/networkgraph/integrations.js", [f["parts/Globals.js"]], function (f) {
        f.networkgraphIntegrations = {
            verlet: {
                attractiveForceFunction: function (b, a) { return (a - b) / b }, repulsiveForceFunction: function (b, a) { return (a - b) / b * (a > b ? 1 : 0) }, barycenter: function () {
                    var b = this.options.gravitationalConstant, a = this.barycenter.xFactor, e = this.barycenter.yFactor; a = (a - (this.box.left + this.box.width) / 2) * b; e =
                        (e - (this.box.top + this.box.height) / 2) * b; this.nodes.forEach(function (d) { d.fixedPosition || (d.plotX -= a / d.mass / d.degree, d.plotY -= e / d.mass / d.degree) })
                }, repulsive: function (b, a, e) { a = a * this.diffTemperature / b.mass / b.degree; b.fixedPosition || (b.plotX += e.x * a, b.plotY += e.y * a) }, attractive: function (b, a, e) {
                    var d = b.getMass(), g = -e.x * a * this.diffTemperature; a = -e.y * a * this.diffTemperature; b.fromNode.fixedPosition || (b.fromNode.plotX -= g * d.fromNode / b.fromNode.degree, b.fromNode.plotY -= a * d.fromNode / b.fromNode.degree); b.toNode.fixedPosition ||
                        (b.toNode.plotX += g * d.toNode / b.toNode.degree, b.toNode.plotY += a * d.toNode / b.toNode.degree)
                }, integrate: function (b, a) { var e = -b.options.friction, d = b.options.maxSpeed, g = (a.plotX + a.dispX - a.prevX) * e; e *= a.plotY + a.dispY - a.prevY; var f = Math.abs, q = f(g) / (g || 1); f = f(e) / (e || 1); g = q * Math.min(d, Math.abs(g)); e = f * Math.min(d, Math.abs(e)); a.prevX = a.plotX + a.dispX; a.prevY = a.plotY + a.dispY; a.plotX += g; a.plotY += e; a.temperature = b.vectorLength({ x: g, y: e }) }, getK: function (b) { return Math.pow(b.box.width * b.box.height / b.nodes.length, .5) }
            },
            euler: {
                attractiveForceFunction: function (b, a) { return b * b / a }, repulsiveForceFunction: function (b, a) { return a * a / b }, barycenter: function () { var b = this.options.gravitationalConstant, a = this.barycenter.xFactor, e = this.barycenter.yFactor; this.nodes.forEach(function (d) { if (!d.fixedPosition) { var g = d.getDegree(); g *= 1 + g / 2; d.dispX += (a - d.plotX) * b * g / d.degree; d.dispY += (e - d.plotY) * b * g / d.degree } }) }, repulsive: function (b, a, e, d) { b.dispX += e.x / d * a / b.degree; b.dispY += e.y / d * a / b.degree }, attractive: function (b, a, e, d) {
                    var g = b.getMass(),
                    f = e.x / d * a; a *= e.y / d; b.fromNode.fixedPosition || (b.fromNode.dispX -= f * g.fromNode / b.fromNode.degree, b.fromNode.dispY -= a * g.fromNode / b.fromNode.degree); b.toNode.fixedPosition || (b.toNode.dispX += f * g.toNode / b.toNode.degree, b.toNode.dispY += a * g.toNode / b.toNode.degree)
                }, integrate: function (b, a) {
                    a.dispX += a.dispX * b.options.friction; a.dispY += a.dispY * b.options.friction; var e = a.temperature = b.vectorLength({ x: a.dispX, y: a.dispY }); 0 !== e && (a.plotX += a.dispX / e * Math.min(Math.abs(a.dispX), b.temperature), a.plotY += a.dispY / e * Math.min(Math.abs(a.dispY),
                        b.temperature))
                }, getK: function (b) { return Math.pow(b.box.width * b.box.height / b.nodes.length, .3) }
            }
        }
    }); m(f, "modules/networkgraph/QuadTree.js", [f["parts/Globals.js"], f["parts/Utilities.js"]], function (f, b) {
        b = b.extend; var a = f.QuadTreeNode = function (a) { this.box = a; this.boxSize = Math.min(a.width, a.height); this.nodes = []; this.body = this.isInternal = !1; this.isEmpty = !0 }; b(a.prototype, {
            insert: function (e, d) {
                this.isInternal ? this.nodes[this.getBoxPosition(e)].insert(e, d - 1) : (this.isEmpty = !1, this.body ? d ? (this.isInternal =
                    !0, this.divideBox(), !0 !== this.body && (this.nodes[this.getBoxPosition(this.body)].insert(this.body, d - 1), this.body = !0), this.nodes[this.getBoxPosition(e)].insert(e, d - 1)) : (d = new a({ top: e.plotX, left: e.plotY, width: .1, height: .1 }), d.body = e, d.isInternal = !1, this.nodes.push(d)) : (this.isInternal = !1, this.body = e))
            }, updateMassAndCenter: function () {
                var a = 0, d = 0, b = 0; this.isInternal ? (this.nodes.forEach(function (e) { e.isEmpty || (a += e.mass, d += e.plotX * e.mass, b += e.plotY * e.mass) }), d /= a, b /= a) : this.body && (a = this.body.mass, d = this.body.plotX,
                    b = this.body.plotY); this.mass = a; this.plotX = d; this.plotY = b
            }, divideBox: function () { var b = this.box.width / 2, d = this.box.height / 2; this.nodes[0] = new a({ left: this.box.left, top: this.box.top, width: b, height: d }); this.nodes[1] = new a({ left: this.box.left + b, top: this.box.top, width: b, height: d }); this.nodes[2] = new a({ left: this.box.left + b, top: this.box.top + d, width: b, height: d }); this.nodes[3] = new a({ left: this.box.left, top: this.box.top + d, width: b, height: d }) }, getBoxPosition: function (a) {
                var d = a.plotY < this.box.top + this.box.height /
                    2; return a.plotX < this.box.left + this.box.width / 2 ? d ? 0 : 3 : d ? 1 : 2
            }
        }); f = f.QuadTree = function (b, d, f, n) { this.box = { left: b, top: d, width: f, height: n }; this.maxDepth = 25; this.root = new a(this.box, "0"); this.root.isInternal = !0; this.root.isRoot = !0; this.root.divideBox() }; b(f.prototype, {
            insertNodes: function (a) { a.forEach(function (a) { this.root.insert(a, this.maxDepth) }, this) }, visitNodeRecursive: function (a, d, b) {
                var e; a || (a = this.root); a === this.root && d && (e = d(a)); !1 !== e && (a.nodes.forEach(function (a) {
                    if (a.isInternal) {
                        d && (e = d(a));
                        if (!1 === e) return; this.visitNodeRecursive(a, d, b)
                    } else a.body && d && d(a.body); b && b(a)
                }, this), a === this.root && b && b(a))
            }, calculateMassAndCenter: function () { this.visitNodeRecursive(null, null, function (a) { a.updateMassAndCenter() }) }
        })
    }); m(f, "modules/networkgraph/layouts.js", [f["parts/Chart.js"], f["parts/Globals.js"], f["parts/Utilities.js"]], function (f, b, a) {
        var e = a.addEvent, d = a.clamp, g = a.defined, n = a.extend, q = a.isFunction, k = a.pick, p = a.setAnimation; b.layouts = { "reingold-fruchterman": function () { } }; n(b.layouts["reingold-fruchterman"].prototype,
            {
                init: function (c) { this.options = c; this.nodes = []; this.links = []; this.series = []; this.box = { x: 0, y: 0, width: 0, height: 0 }; this.setInitialRendering(!0); this.integration = b.networkgraphIntegrations[c.integration]; this.enableSimulation = c.enableSimulation; this.attractiveForce = k(c.attractiveForce, this.integration.attractiveForceFunction); this.repulsiveForce = k(c.repulsiveForce, this.integration.repulsiveForceFunction); this.approximation = c.approximation }, updateSimulation: function (c) { this.enableSimulation = k(c, this.options.enableSimulation) },
                start: function () { var c = this.series, a = this.options; this.currentStep = 0; this.forces = c[0] && c[0].forces || []; this.chart = c[0] && c[0].chart; this.initialRendering && (this.initPositions(), c.forEach(function (c) { c.finishedAnimating = !0; c.render() })); this.setK(); this.resetSimulation(a); this.enableSimulation && this.step() }, step: function () {
                    var c = this, a = this.series; c.currentStep++; "barnes-hut" === c.approximation && (c.createQuadTree(), c.quadTree.calculateMassAndCenter()); c.forces.forEach(function (a) { c[a + "Forces"](c.temperature) });
                    c.applyLimits(c.temperature); c.temperature = c.coolDown(c.startTemperature, c.diffTemperature, c.currentStep); c.prevSystemTemperature = c.systemTemperature; c.systemTemperature = c.getSystemTemperature(); c.enableSimulation && (a.forEach(function (c) { c.chart && c.render() }), c.maxIterations-- && isFinite(c.temperature) && !c.isStable() ? (c.simulation && b.win.cancelAnimationFrame(c.simulation), c.simulation = b.win.requestAnimationFrame(function () { c.step() })) : c.simulation = !1)
                }, stop: function () { this.simulation && b.win.cancelAnimationFrame(this.simulation) },
                setArea: function (c, a, b, d) { this.box = { left: c, top: a, width: b, height: d } }, setK: function () { this.k = this.options.linkLength || this.integration.getK(this) }, addElementsToCollection: function (c, a) { c.forEach(function (c) { -1 === a.indexOf(c) && a.push(c) }) }, removeElementFromCollection: function (c, a) { c = a.indexOf(c); -1 !== c && a.splice(c, 1) }, clear: function () { this.nodes.length = 0; this.links.length = 0; this.series.length = 0; this.resetSimulation() }, resetSimulation: function () {
                    this.forcedStop = !1; this.systemTemperature = 0; this.setMaxIterations();
                    this.setTemperature(); this.setDiffTemperature()
                }, restartSimulation: function () { this.simulation ? this.resetSimulation() : (this.setInitialRendering(!1), this.enableSimulation ? this.start() : this.setMaxIterations(1), this.chart && this.chart.redraw(), this.setInitialRendering(!0)) }, setMaxIterations: function (c) { this.maxIterations = k(c, this.options.maxIterations) }, setTemperature: function () { this.temperature = this.startTemperature = Math.sqrt(this.nodes.length) }, setDiffTemperature: function () {
                    this.diffTemperature = this.startTemperature /
                        (this.options.maxIterations + 1)
                }, setInitialRendering: function (c) { this.initialRendering = c }, createQuadTree: function () { this.quadTree = new b.QuadTree(this.box.left, this.box.top, this.box.width, this.box.height); this.quadTree.insertNodes(this.nodes) }, initPositions: function () { var c = this.options.initialPositions; q(c) ? (c.call(this), this.nodes.forEach(function (c) { g(c.prevX) || (c.prevX = c.plotX); g(c.prevY) || (c.prevY = c.plotY); c.dispX = 0; c.dispY = 0 })) : "circle" === c ? this.setCircularPositions() : this.setRandomPositions() },
                setCircularPositions: function () {
                    function c(a) { a.linksFrom.forEach(function (a) { g[a.toNode.id] || (g[a.toNode.id] = !0, f.push(a.toNode), c(a.toNode)) }) } var a = this.box, b = this.nodes, d = 2 * Math.PI / (b.length + 1), e = b.filter(function (c) { return 0 === c.linksTo.length }), f = [], g = {}, p = this.options.initialPositionRadius; e.forEach(function (a) { f.push(a); c(a) }); f.length ? b.forEach(function (c) { -1 === f.indexOf(c) && f.push(c) }) : f = b; f.forEach(function (c, b) {
                        c.plotX = c.prevX = k(c.plotX, a.width / 2 + p * Math.cos(b * d)); c.plotY = c.prevY = k(c.plotY,
                            a.height / 2 + p * Math.sin(b * d)); c.dispX = 0; c.dispY = 0
                    })
                }, setRandomPositions: function () { function c(c) { c = c * c / Math.PI; return c -= Math.floor(c) } var a = this.box, b = this.nodes, d = b.length + 1; b.forEach(function (b, h) { b.plotX = b.prevX = k(b.plotX, a.width * c(h)); b.plotY = b.prevY = k(b.plotY, a.height * c(d + h)); b.dispX = 0; b.dispY = 0 }) }, force: function (c) { this.integration[c].apply(this, Array.prototype.slice.call(arguments, 1)) }, barycenterForces: function () { this.getBarycenter(); this.force("barycenter") }, getBarycenter: function () {
                    var c =
                        0, a = 0, b = 0; this.nodes.forEach(function (d) { a += d.plotX * d.mass; b += d.plotY * d.mass; c += d.mass }); return this.barycenter = { x: a, y: b, xFactor: a / c, yFactor: b / c }
                }, barnesHutApproximation: function (c, a) { var b = this.getDistXY(c, a), d = this.vectorLength(b); if (c !== a && 0 !== d) if (a.isInternal) if (a.boxSize / d < this.options.theta && 0 !== d) { var h = this.repulsiveForce(d, this.k); this.force("repulsive", c, h * a.mass, b, d); var e = !1 } else e = !0; else h = this.repulsiveForce(d, this.k), this.force("repulsive", c, h * a.mass, b, d); return e }, repulsiveForces: function () {
                    var c =
                        this; "barnes-hut" === c.approximation ? c.nodes.forEach(function (a) { c.quadTree.visitNodeRecursive(null, function (b) { return c.barnesHutApproximation(a, b) }) }) : c.nodes.forEach(function (a) { c.nodes.forEach(function (b) { if (a !== b && !a.fixedPosition) { var d = c.getDistXY(a, b); var h = c.vectorLength(d); if (0 !== h) { var e = c.repulsiveForce(h, c.k); c.force("repulsive", a, e * b.mass, d, h) } } }) })
                }, attractiveForces: function () {
                    var c = this, a, b, d; c.links.forEach(function (h) {
                        h.fromNode && h.toNode && (a = c.getDistXY(h.fromNode, h.toNode), b = c.vectorLength(a),
                            0 !== b && (d = c.attractiveForce(b, c.k), c.force("attractive", h, d, a, b)))
                    })
                }, applyLimits: function () { var c = this; c.nodes.forEach(function (a) { a.fixedPosition || (c.integration.integrate(c, a), c.applyLimitBox(a, c.box), a.dispX = 0, a.dispY = 0) }) }, applyLimitBox: function (c, a) { var b = c.radius; c.plotX = d(c.plotX, a.left + b, a.width - b); c.plotY = d(c.plotY, a.top + b, a.height - b) }, coolDown: function (c, a, b) { return c - a * b }, isStable: function () { return .00001 > Math.abs(this.systemTemperature - this.prevSystemTemperature) || 0 >= this.temperature },
                getSystemTemperature: function () { return this.nodes.reduce(function (c, a) { return c + a.temperature }, 0) }, vectorLength: function (c) { return Math.sqrt(c.x * c.x + c.y * c.y) }, getDistR: function (c, a) { c = this.getDistXY(c, a); return this.vectorLength(c) }, getDistXY: function (c, a) { var b = c.plotX - a.plotX; c = c.plotY - a.plotY; return { x: b, y: c, absX: Math.abs(b), absY: Math.abs(c) } }
            }); e(f, "predraw", function () { this.graphLayoutsLookup && this.graphLayoutsLookup.forEach(function (c) { c.stop() }) }); e(f, "render", function () {
                function c(c) {
                    c.maxIterations-- &&
                    isFinite(c.temperature) && !c.isStable() && !c.enableSimulation && (c.beforeStep && c.beforeStep(), c.step(), b = !1, a = !0)
                } var a = !1; if (this.graphLayoutsLookup) { p(!1, this); for (this.graphLayoutsLookup.forEach(function (c) { c.start() }); !b;) { var b = !0; this.graphLayoutsLookup.forEach(c) } a && this.series.forEach(function (c) { c && c.layout && c.render() }) }
            }); e(f, "beforePrint", function () { this.graphLayoutsLookup && (this.graphLayoutsLookup.forEach(function (c) { c.updateSimulation(!1) }), this.redraw()) }); e(f, "afterPrint", function () {
                this.graphLayoutsLookup &&
                this.graphLayoutsLookup.forEach(function (c) { c.updateSimulation() }); this.redraw()
            })
    }); m(f, "modules/networkgraph/draggable-nodes.js", [f["parts/Chart.js"], f["parts/Globals.js"], f["parts/Utilities.js"]], function (f, b, a) {
        var e = a.addEvent; b.dragNodesMixin = {
            onMouseDown: function (a, b) { b = this.chart.pointer.normalize(b); a.fixedPosition = { chartX: b.chartX, chartY: b.chartY, plotX: a.plotX, plotY: a.plotY }; a.inDragMode = !0 }, onMouseMove: function (a, b) {
                if (a.fixedPosition && a.inDragMode) {
                    var d = this.chart; b = d.pointer.normalize(b);
                    var e = a.fixedPosition.chartX - b.chartX, f = a.fixedPosition.chartY - b.chartY; b = d.graphLayoutsLookup; if (5 < Math.abs(e) || 5 < Math.abs(f)) e = a.fixedPosition.plotX - e, f = a.fixedPosition.plotY - f, d.isInsidePlot(e, f) && (a.plotX = e, a.plotY = f, a.hasDragged = !0, this.redrawHalo(a), b.forEach(function (a) { a.restartSimulation() }))
                }
            }, onMouseUp: function (a, b) { a.fixedPosition && a.hasDragged && (this.layout.enableSimulation ? this.layout.start() : this.chart.redraw(), a.inDragMode = a.hasDragged = !1, this.options.fixedDraggable || delete a.fixedPosition) },
            redrawHalo: function (a) { a && this.halo && this.halo.attr({ d: a.haloPath(this.options.states.hover.halo.size) }) }
        }; e(f, "load", function () {
            var a = this, b, f, q; a.container && (b = e(a.container, "mousedown", function (b) {
                var d = a.hoverPoint; d && d.series && d.series.hasDraggableNodes && d.series.options.draggable && (d.series.onMouseDown(d, b), f = e(a.container, "mousemove", function (c) { return d && d.series && d.series.onMouseMove(d, c) }), q = e(a.container.ownerDocument, "mouseup", function (c) {
                    f(); q(); return d && d.series && d.series.onMouseUp(d,
                        c)
                }))
            })); e(a, "destroy", function () { b() })
        })
    }); m(f, "modules/networkgraph/networkgraph.src.js", [f["parts/Globals.js"], f["parts/Point.js"], f["parts/Utilities.js"]], function (f, b, a) {
        var e = a.addEvent, d = a.css, g = a.defined, n = a.pick; a = a.seriesType; ""; var m = f.seriesTypes, k = f.Series, p = f.dragNodesMixin; a("networkgraph", "line", {
            stickyTracking: !1, inactiveOtherPoints: !0, marker: { enabled: !0, states: { inactive: { opacity: .3, animation: { duration: 50 } } } }, states: { inactive: { linkOpacity: .3, animation: { duration: 50 } } }, dataLabels: {
                formatter: function () { return this.key },
                linkFormatter: function () { return this.point.fromNode.name + "<br>" + this.point.toNode.name }, linkTextPath: { enabled: !0 }, textPath: { enabled: !1 }, style: { transition: "opacity 2000ms" }
            }, link: { color: "rgba(100, 100, 100, 0.5)", width: 1 }, draggable: !0, layoutAlgorithm: { initialPositions: "circle", initialPositionRadius: 1, enableSimulation: !1, theta: .5, maxSpeed: 10, approximation: "none", type: "reingold-fruchterman", integration: "euler", maxIterations: 1E3, gravitationalConstant: .0625, friction: -.981 }, showInLegend: !1
        }, {
            forces: ["barycenter",
                "repulsive", "attractive"], hasDraggableNodes: !0, drawGraph: null, isCartesian: !1, requireSorting: !1, directTouch: !0, noSharedTooltip: !0, pointArrayMap: ["from", "to"], trackerGroups: ["group", "markerGroup", "dataLabelsGroup"], drawTracker: f.TrackerMixin.drawTrackerPoint, animate: null, buildKDTree: f.noop, createNode: f.NodesMixin.createNode, destroy: function () { this.layout.removeElementFromCollection(this, this.layout.series); f.NodesMixin.destroy.call(this) }, init: function () {
                    k.prototype.init.apply(this, arguments); e(this,
                        "updatedData", function () { this.layout && this.layout.stop() }); return this
                }, generatePoints: function () {
                    var c; f.NodesMixin.generatePoints.apply(this, arguments); this.options.nodes && this.options.nodes.forEach(function (a) { this.nodeLookup[a.id] || (this.nodeLookup[a.id] = this.createNode(a.id)) }, this); for (c = this.nodes.length - 1; 0 <= c; c--) { var a = this.nodes[c]; a.degree = a.getDegree(); a.radius = n(a.marker && a.marker.radius, this.options.marker && this.options.marker.radius, 0); this.nodeLookup[a.id] || a.remove() } this.data.forEach(function (a) {
                        a.formatPrefix =
                        "link"
                    }); this.indexateNodes()
                }, getPointsCollection: function () { return this.nodes || [] }, indexateNodes: function () { this.nodes.forEach(function (a, b) { a.index = b }) }, markerAttribs: function (a, b) { b = k.prototype.markerAttribs.call(this, a, b); g(a.plotY) || (b.y = 0); b.x = (a.plotX || 0) - (b.width / 2 || 0); return b }, translate: function () { this.processedXData || this.processData(); this.generatePoints(); this.deferLayout(); this.nodes.forEach(function (a) { a.isInside = !0; a.linksFrom.forEach(function (a) { a.shapeType = "path"; a.y = 1 }) }) }, deferLayout: function () {
                    var a =
                        this.options.layoutAlgorithm, b = this.chart.graphLayoutsStorage, d = this.chart.graphLayoutsLookup, e = this.chart.options.chart; if (this.visible) {
                            b || (this.chart.graphLayoutsStorage = b = {}, this.chart.graphLayoutsLookup = d = []); var l = b[a.type]; l || (a.enableSimulation = g(e.forExport) ? !e.forExport : a.enableSimulation, b[a.type] = l = new f.layouts[a.type], l.init(a), d.splice(l.index, 0, l)); this.layout = l; l.setArea(0, 0, this.chart.plotWidth, this.chart.plotHeight); l.addElementsToCollection([this], l.series); l.addElementsToCollection(this.nodes,
                                l.nodes); l.addElementsToCollection(this.points, l.links)
                        }
                }, render: function () { var a = this.points, b = this.chart.hoverPoint, d = []; this.points = this.nodes; m.line.prototype.render.call(this); this.points = a; a.forEach(function (a) { a.fromNode && a.toNode && (a.renderLink(), a.redrawLink()) }); b && b.series === this && this.redrawHalo(b); this.chart.hasRendered && !this.options.dataLabels.allowOverlap && (this.nodes.concat(this.points).forEach(function (a) { a.dataLabel && d.push(a.dataLabel) }), this.chart.hideOverlappingLabels(d)) }, drawDataLabels: function () {
                    var a =
                        this.options.dataLabels.textPath; k.prototype.drawDataLabels.apply(this, arguments); this.points = this.data; this.options.dataLabels.textPath = this.options.dataLabels.linkTextPath; k.prototype.drawDataLabels.apply(this, arguments); this.points = this.nodes; this.options.dataLabels.textPath = a
                }, pointAttribs: function (a, b) {
                    var c = b || a && a.state || "normal"; b = k.prototype.pointAttribs.call(this, a, c); c = this.options.states[c]; a && !a.isNode && (b = a.getLinkAttributes(), c && (b = {
                        stroke: c.linkColor || b.stroke, dashstyle: c.linkDashStyle ||
                            b.dashstyle, opacity: n(c.linkOpacity, b.opacity), "stroke-width": c.linkColor || b["stroke-width"]
                    })); return b
                }, redrawHalo: p.redrawHalo, onMouseDown: p.onMouseDown, onMouseMove: p.onMouseMove, onMouseUp: p.onMouseUp, setState: function (a, b) { b ? (this.points = this.nodes.concat(this.data), k.prototype.setState.apply(this, arguments), this.points = this.data) : k.prototype.setState.apply(this, arguments); this.layout.simulation || a || this.render() }
        }, {
            setState: f.NodesMixin.setNodeState, init: function () {
                b.prototype.init.apply(this,
                    arguments); this.series.options.draggable && !this.series.chart.styledMode && (e(this, "mouseOver", function () { d(this.series.chart.container, { cursor: "move" }) }), e(this, "mouseOut", function () { d(this.series.chart.container, { cursor: "default" }) })); return this
            }, getDegree: function () { var a = this.isNode ? this.linksFrom.length + this.linksTo.length : 0; return 0 === a ? 1 : a }, getLinkAttributes: function () {
                var a = this.series.options.link, b = this.options; return {
                    "stroke-width": n(b.width, a.width), stroke: b.color || a.color, dashstyle: b.dashStyle ||
                        a.dashStyle, opacity: n(b.opacity, a.opacity, 1)
                }
            }, renderLink: function () { if (!this.graphic && (this.graphic = this.series.chart.renderer.path(this.getLinkPath()).add(this.series.group), !this.series.chart.styledMode)) { var a = this.series.pointAttribs(this); this.graphic.attr(a); (this.dataLabels || []).forEach(function (c) { c && c.attr({ opacity: a.opacity }) }) } }, redrawLink: function () {
                var a = this.getLinkPath(); if (this.graphic) {
                    this.shapeArgs = { d: a }; if (!this.series.chart.styledMode) {
                        var b = this.series.pointAttribs(this); this.graphic.attr(b);
                        (this.dataLabels || []).forEach(function (a) { a && a.attr({ opacity: b.opacity }) })
                    } this.graphic.animate(this.shapeArgs); var d = a[0]; a = a[1]; "M" === d[0] && "L" === a[0] && (this.plotX = (d[1] + a[1]) / 2, this.plotY = (d[2] + a[2]) / 2)
                }
            }, getMass: function () { var a = this.fromNode.mass, b = this.toNode.mass, d = a + b; return { fromNode: 1 - a / d, toNode: 1 - b / d } }, getLinkPath: function () { var a = this.fromNode, b = this.toNode; a.plotX > b.plotX && (a = this.toNode, b = this.fromNode); return [["M", a.plotX || 0, a.plotY || 0], ["L", b.plotX || 0, b.plotY || 0]] }, isValid: function () {
                return !this.isNode ||
                    g(this.id)
            }, remove: function (a, b) {
                var c = this.series, d = c.options.nodes || [], e, f = d.length; if (this.isNode) {
                    c.points = [];[].concat(this.linksFrom).concat(this.linksTo).forEach(function (a) { e = a.fromNode.linksFrom.indexOf(a); -1 < e && a.fromNode.linksFrom.splice(e, 1); e = a.toNode.linksTo.indexOf(a); -1 < e && a.toNode.linksTo.splice(e, 1); k.prototype.removePoint.call(c, c.data.indexOf(a), !1, !1) }); c.points = c.data.slice(); for (c.nodes.splice(c.nodes.indexOf(this), 1); f--;)if (d[f].id === this.options.id) {
                        c.options.nodes.splice(f,
                            1); break
                    } this && this.destroy(); c.isDirty = !0; c.isDirtyData = !0; a && c.chart.redraw(a)
                } else c.removePoint(c.data.indexOf(this), a, b)
            }, destroy: function () { this.isNode && this.linksFrom.concat(this.linksTo).forEach(function (a) { a.destroyElements && a.destroyElements() }); this.series.layout.removeElementFromCollection(this, this.series.layout[this.isNode ? "nodes" : "links"]); return b.prototype.destroy.apply(this, arguments) }
        }); ""
    }); m(f, "masters/modules/networkgraph.src.js", [], function () { })
});
//# sourceMappingURL=networkgraph.js.map