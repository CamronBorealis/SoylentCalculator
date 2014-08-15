SoylentCalculator
=================

This is a program designed to help calculate optimal mixes for soylent ingredients. It's mostly a proof of concept, but can be extended to include nutrients.

It's a genetic algorithm. You run a simulation which has cycles. Each simulation starts with a really bad recipe, and then refines this. Each cycle within a simulation spawns a number of mutations of the simulation's best recipe so far. If any of the mutations are better than the simulation's best, it is promoted to be the best and the process continues. Because recipes can vary, we run several simulations and take the best global recipe and dump it to the Output window.

To determine how good a recipe is, it's assigned a score. Scores are compared when determining if a recipe is better than another.
