﻿// <copyright file="LearnablesConsumeHandler.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameLogic.PlayerActions.ItemConsumeActions;

/// <summary>
/// Consume handler for items (e.g. scrolls, orbs) which add a skill when being consumed.
/// </summary>
public class LearnablesConsumeHandler : BaseConsumeHandler
{
    /// <inheritdoc/>
    public override async ValueTask<bool> ConsumeItemAsync(Player player, Item item, Item? targetItem, FruitUsage fruitUsage)
    {
        var skill = this.GetLearnableSkill(item, player.GameContext.Configuration);

        if (skill is null || player.SkillList!.ContainsSkill(skill.Number.ToUnsigned()))
        {
            return false;
        }

        if (!await base.ConsumeItemAsync(player, item, targetItem, fruitUsage))
        {
            return false;
        }

        await player.SkillList.AddLearnedSkillAsync(skill).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc />
    protected override bool CheckPreconditions(Player player, Item item)
    {
        return base.CheckPreconditions(player, item)
               && player.CompliesRequirements(item);
    }

    /// <summary>
    /// Gets the learnable skill.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    /// <returns>The skill to learn.</returns>
    protected virtual Skill? GetLearnableSkill(Item item, GameConfiguration gameConfiguration)
    {
        return item.Definition?.Skill;
    }
}